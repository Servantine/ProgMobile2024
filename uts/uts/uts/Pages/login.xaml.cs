namespace uts.Pages;

public partial class login : ContentPage
{
	public login()
	{
		InitializeComponent();
	}
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {

        await DisplayAlert("Login", "Anda berhasil login!", "OK");
        await Navigation.PushAsync(new MainPage());

    }
}