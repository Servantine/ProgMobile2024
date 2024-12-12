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
                App.AuthToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InB1dHJpYXZhbnRpbiIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczMzk3NzgxOCwiZXhwIjoxNzMzOTc5NjE4LCJpYXQiOjE3MzM5Nzc4MTh9.cRsT2cBn3PlyZZ2LwCqHKZVAG2J2LB07MFQ8KEGIJaE"; // Simpan token ke App.AuthToken
                await Application.Current.MainPage.DisplayAlert("Login Successful", $"Token: {App.AuthToken}", "OK");
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

                // Navigasi ke halaman utama
                await Navigation.PushAsync(new MainPage());
            }

        }
        catch (Exception ex)
        {

        }



    }
}