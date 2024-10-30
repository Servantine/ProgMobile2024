using System.Collections.Generic;
using Microsoft.Maui.Controls;
using uts.Models;
using uts.Pages;
namespace uts
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;
        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadCategories();
        }
        private async void LoadCategories()
        {
            try
            {
                var categories = await _apiService.GetCategoriesAsync();
                CategoriesCollectionView.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }
        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            string categoryname = await DisplayPromptAsync("Add Category", "Enter the category name:");
            string categorydescription = await DisplayPromptAsync("Add Category", "Enter the category description:");

            if (!string.IsNullOrWhiteSpace(categoryname))
            {
                var newCategory = new Categories
                {
                    name = categoryname,
                    description = categorydescription
                };

                bool success = await _apiService.PostCategoryAsync(newCategory);
                if (success)
                {
                    await DisplayAlert("Success", "Category added successfully!", "OK");
                    LoadCategories(); 
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add the category.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid category name and description.", "OK");
            }
        }
        private async void OnEditCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Categories;
            if (category == null) return;

           
            string newName = await DisplayPromptAsync("Edit Category", "Enter the new category name:", initialValue: category.name);
            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Invalid Input", "Please enter a valid name.", "OK");
                return;
            }

            
            string newDescription = await DisplayPromptAsync("Edit Category", "Enter the new category description:", initialValue: category.description);
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                await DisplayAlert("Invalid Input", "Please enter a valid description.", "OK");
                return;
            }

            
            category.name = newName;
            category.description = newDescription;

            
            bool success = await _apiService.PutCategoryAsync(category);
            if (success)
            {
                await DisplayAlert("Success", "Category updated successfully!", "OK");
                LoadCategories(); 
            }
            else
            {
                await DisplayAlert("Error", "Failed to update the category.", "OK");
            }
        }


        
        private async void OnDeleteCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Categories;
            if (category == null) return;

            
            bool confirm = await DisplayAlert("Delete Category", $"Are you sure you want to delete category '{category.name}'?", "Yes", "No");
            if (!confirm) return;

           
            bool success = await _apiService.DeleteCategoryAsync(category.categoryId);
            if (success)
            {
                await DisplayAlert("Success", "Category deleted successfully!", "OK");
                LoadCategories();
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete the category.", "OK");
            }
        }
        private async void OnCoursesButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CoursesPage());
        }
        private async void OnSearchCategoryByIdClicked(object sender, EventArgs e)
        {
            if (int.TryParse(CategoryIdEntry.Text, out int categoryId))
            {
                var category = await _apiService.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    CategorySearchResultStack.IsVisible = true;
                    CategorySearchResultLabel.Text = $"Category ID: {category.categoryId}\n" +
                                                     $"Name: {category.name}\n" +
                                                     $"Description: {category.description}";
                }
                else
                {
                    await DisplayAlert("Not Found", "Category with the specified ID not found.", "OK");
                    CategorySearchResultStack.IsVisible = false;
                }
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid numeric ID.", "OK");
            }
        }
    }

}
