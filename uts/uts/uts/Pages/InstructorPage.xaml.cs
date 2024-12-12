using uts.Models;

namespace uts.Pages;

public partial class InstructorPage : ContentPage
{
    private readonly ApiService _apiService;
    public InstructorPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
        LoadData();
    }
    private async void LoadData()
    {
        try
        {
            // Fetch data from APIs
            var users = await _apiService.GetUsersAsync();
            var instructors = await _apiService.GetInstructorsAsync();

            // Join data
            var userInstructorData = (from user in users
                                      join instructor in instructors
                                      on user.UserName equals instructor.userName
                                      select new UserInstructorViewModel
                                      {
                                          Id = user.Id,
                                          Email = user.Email,
                                          UserName = user.UserName,
                                          FullName = user.FullName,
                                          InstructorId = instructor.instructorId
                                      }).ToList();

            // Bind to CollectionView
            UsersCollectionView.ItemsSource = userInstructorData;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
    }
    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        string email = await DisplayPromptAsync("Add User", "Enter the user's email:");
        string username = await DisplayPromptAsync("Add User", "Enter the username:");
        string password = await DisplayPromptAsync("Add User", "Enter the password:");
        string fullName = await DisplayPromptAsync("Add User", "Enter the user's full name:");

        if (!string.IsNullOrWhiteSpace(email) &&
            !string.IsNullOrWhiteSpace(username) &&
            !string.IsNullOrWhiteSpace(password) &&
            !string.IsNullOrWhiteSpace(fullName))
        {
            var newUser = new Users
            {
                Email = email,
                UserName = username,
                Password = password,
                FullName = fullName
            };

            bool success = await _apiService.PostUsersAsync(newUser);
            if (success)
            {
                await DisplayAlert("Success", "User added successfully!", "OK");
                LoadData(); // Fungsi untuk me-refresh daftar pengguna.
            }
            else
            {
                await DisplayAlert("Error", "Failed to add the user.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please fill in all fields correctly.", "OK");
        }
    }
    private async void OnAssignRoleClicked(object sender, EventArgs e)
    {
        try
        {
            // Ambil userName dari CommandParameter
            var button = sender as Button;
            if (button?.CommandParameter is not string userName || string.IsNullOrWhiteSpace(userName))
            {
                await DisplayAlert("Error", "Failed to retrieve user information.", "OK");
                return;
            }

            // Tampilkan dropdown untuk memilih role
            string selectedRole = await Application.Current.MainPage.DisplayActionSheet(
                "Select a Role",
                "Cancel",
                null,
                "admin",
                "dosen");

            // Batalkan jika pengguna tidak memilih
            if (selectedRole == "Cancel" || string.IsNullOrWhiteSpace(selectedRole))
            {
                return;
            }

            // Membentuk payload untuk API
            var roleAssignmentPayload = new
            {
                userName = userName,
                roleName = selectedRole
            };

            // Kirim data ke API
            var isRoleAssigned = await _apiService.AssignRoleAsync(roleAssignmentPayload);
            if (isRoleAssigned)
            {
                await DisplayAlert("Success", $"Role '{selectedRole}' assigned to {userName}.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to assign role.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }




}