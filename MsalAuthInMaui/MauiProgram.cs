global using Microsoft.Extensions.Configuration;
global using System.Reflection;

namespace MsalAuthInMaui
{
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

            var executingAssembly = Assembly.GetExecutingAssembly();

            using var stream = executingAssembly.GetManifestResourceStream("MsalAuthInMaui.appsettings.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Services.AddTransient<MainPage>();

            builder.Configuration
                .AddConfiguration(configuration);
            return builder.Build();
        }
    }
}