using uts.Pages;

namespace uts
{
    public partial class App : Application
    {
        public static string AuthToken { get; set; } = string.Empty; // Token global
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            MainPage = new NavigationPage(new login());
        }
    }
}
