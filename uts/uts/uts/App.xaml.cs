using uts.Pages;

namespace uts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            MainPage = new NavigationPage(new login());
        }
    }
}
