namespace People;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register the PersonRepository
        builder.Services.AddSingleton<PersonRepository>(serviceProvider =>
        {
            // Provide the _dbPath dynamically (e.g., platform-specific storage path)
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "people.db");
            return new PersonRepository(dbPath);
        });

        return builder.Build();
    }
}
