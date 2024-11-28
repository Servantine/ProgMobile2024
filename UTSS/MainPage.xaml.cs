using System.Collections.ObjectModel;
using UTSS.Models;

namespace UTSS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        // Muat semua kategori dari database
        private async void LoadCategories()
        {
            try
            {
                var categories = await App.Database.GetCategoriesAsync();
                if (categories != null)
                {
                    CategoriesListView.ItemsSource = new ObservableCollection<Categories>(categories);
                }
                else
                {
                    await DisplayAlert("Info", "No categories available.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories. Error: {ex.Message}", "OK");
            }
        }

        // Tambahkan kategori baru
        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NewNameEntry.Text) && !string.IsNullOrWhiteSpace(NewDescriptionEntry.Text))
                {
                    var newCategory = new Categories
                    {
                        name = NewNameEntry.Text.Trim(),
                        description = NewDescriptionEntry.Text.Trim()
                    };

                    await App.Database.AddCategoryAsync(newCategory);

                    // Bersihkan input dan refresh list
                    NewNameEntry.Text = string.Empty;
                    NewDescriptionEntry.Text = string.Empty;
                    LoadCategories();

                    await DisplayAlert("Success", "Category added successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Please fill both fields.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add category. Error: {ex.Message}", "OK");
            }
        }

        // Edit kategori menggunakan DisplayPromptAsync
        private async void OnCategoryTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Categories selectedCategory)
            {
                try
                {
                    // Prompt untuk mengedit nama kategori
                    string newName = await DisplayPromptAsync(
                        "Edit Category Name",
                        $"Edit the name for '{selectedCategory.name}':",
                        initialValue: selectedCategory.name,
                        placeholder: "Enter new name"
                    );

                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        // Prompt untuk mengedit deskripsi kategori
                        string newDescription = await DisplayPromptAsync(
                            "Edit Category Description",
                            $"Edit the description for '{selectedCategory.name}':",
                            initialValue: selectedCategory.description,
                            placeholder: "Enter new description"
                        );

                        if (!string.IsNullOrWhiteSpace(newDescription))
                        {
                            // Update kategori di database
                            selectedCategory.name = newName.Trim();
                            selectedCategory.description = newDescription.Trim();
                            await App.Database.UpdateCategoryAsync(selectedCategory);

                            // Refresh ListView
                            LoadCategories();
                            await DisplayAlert("Success", "Category updated successfully.", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Info", "No changes were made to the description.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Info", "No changes were made to the name.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to update category. Error: {ex.Message}", "OK");
                }
            }

            // Hapus seleksi di ListView
            CategoriesListView.SelectedItem = null;
        }

        // Hapus kategori
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var categoryId = (int)button.CommandParameter;

                // Ambil kategori dari database berdasarkan categoryId
                var category = await App.Database.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    // Tampilkan konfirmasi untuk menghapus kategori
                    var confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete the category '{category.name}'?", "Yes", "No");
                    if (confirm)
                    {
                        // Hapus kategori dari database
                        await App.Database.DeleteCategoryAsync(category);

                        // Refresh ListView
                        LoadCategories();
                        await DisplayAlert("Success", "Category deleted successfully.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Category not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete category. Error: {ex.Message}", "OK");
            }
        }
    }
}
