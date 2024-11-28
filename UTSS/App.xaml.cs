using UTSS.Helpers;

namespace UTSS
{
    public partial class App : Application
    {
        public static DatabaseHelper Database { get; private set; }

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Categories.db3");
            Database = new DatabaseHelper(dbPath);

            MainPage = new AppShell();
        }
    }
}