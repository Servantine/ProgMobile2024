namespace uts.Pages;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using uts.Models;

public partial class login : ContentPage
{

    public static string Token;
	public login()
	{
		InitializeComponent();
	}
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {

        var userName = UsernameEntry.Text;
        var password = PasswordEntry.Text;


        try
        {
            // Melakukan login menggunakan token-based authentication
            var httpClient = new HttpClient();
            var loginData = new { userName, password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://actbackendseervices.azurewebsites.net/api/login", content);

            if (response.IsSuccessStatusCode)
            {

                var responseData = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());
                
                
                // Simpan token menggunakan Secure Storage
                if (!string.IsNullOrEmpty(responseData.Token))
                {
                    await SecureStorage.SetAsync("authToken", responseData.Token);
                    Console.WriteLine("Token securely stored.");
                }
                Console.WriteLine("Token saved securely."); // Tambahkan log untuk memverifikasi

                Console.WriteLine($"Token saved: {App.AuthToken}"); // Tambahkan log untuk memverifikasi
                //var responseData = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());
                //App.AuthToken = responseData.Token;
                if (userName.ToLower() == "servantin0123@gmail.com")
                {
                    App.AuthToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InB1dHJpYXZhbnRpbiIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczNDA2MzMyNCwiZXhwIjoxNzM0MDY1MTI0LCJpYXQiOjE3MzQwNjMzMjR9.W0Mv8nSox5mvgzR-hnbPLaY9hyQQZ7WQROmgz42ystM"; // Simpan token ke App.AuthToken
                    await Application.Current.MainPage.DisplayAlert("Login Successful", $"Token: {App.AuthToken}", "OK");
                    await Application.Current.MainPage.DisplayAlert("Login Successful", "Welcome Admin!", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else if (userName.ToLower() == "thelordofenvy@gmail.com")
                {
                    App.AuthToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InB1dHJpYXZhbnRpbiIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczNDA2MzMyNCwiZXhwIjoxNzM0MDY1MTI0LCJpYXQiOjE3MzQwNjMzMjR9.W0Mv8nSox5mvgzR-hnbPLaY9hyQQZ7WQROmgz42ystM"; // Simpan token ke App.AuthToken
                    await Application.Current.MainPage.DisplayAlert("Login Successful", $"Token: {App.AuthToken}", "OK");
                    await Application.Current.MainPage.DisplayAlert("Login Successful", "Welcome Dosen!", "OK");
                    await Navigation.PushAsync(new DosenPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Unsuccessful", "Please Try again", "OK");
                    await Navigation.PushAsync(new login());
                }
                // Navigasi ke halaman utama
                
            }

        }
        catch (Exception ex)
        {

        }



    }
}