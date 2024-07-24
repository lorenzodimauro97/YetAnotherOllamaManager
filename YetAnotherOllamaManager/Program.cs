namespace YetAnotherOllamaManager;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Extensions;
using Photino.Blazor;
using Services;
using System;
using System.IO;

internal class Program
{ 
    [STAThread] private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path for the JSON file.
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Read the appsettings.json file.
            .Build();
        
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.Services
            .AddLogging();
            
        appBuilder.Services.AddMudServicesWithExtensions();
        appBuilder.Services.AddTransient<OllamaService>();
        appBuilder.Services.AddScoped<StatusUpdateService>();
        appBuilder.Services.AddSingleton<IConfiguration>(provider => configuration);
        appBuilder.Services.AddHttpClient();

        // register root component and selector
        appBuilder.RootComponents.Add<App>("app");
            
        var app = appBuilder.Build();

        // customize window
        app.MainWindow
            .SetIconFile("favicon.ico")
            .SetTitle("Yet Another Ollama Manager");

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();

    }
}