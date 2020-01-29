using System;
using IotHome.Service.Model;
using IotHome.Service.ReadingsMonitor;
using IotHome.Service.ReadingsMonitor.Configuration;
using IotHome.Service.Services;
using IotHome.Service.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace IotHome.Service.ReadingsMonitor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            RegisterConfiguration(builder.Services);
            RegisterDependencies(builder.Services);
        }

        private static void RegisterConfiguration(IServiceCollection services)
        {
            var config = BuildConfigurationRoot();

            var appSettings = config.GetConfigSection<AppSettings>("Values");

            services.AddSingleton(appSettings);
            services.AddSingleton(new StorageConfiguration
            {
                ConnectionString = appSettings.BlobStorageConnectionString,
                ContainerName = appSettings.StorageContainerName,
                ParentDirectory = appSettings.StorageDirectory
            });
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<IAppNotifier, AppNotifier>();
            services.AddSingleton<IStorageHelper, StorageHelper>();
        }

        private static IConfiguration BuildConfigurationRoot()
        {
            var basePath = IsDevelopmentEnvironment()
                ? Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot")
                : $"{Environment.GetEnvironmentVariable("HOME")}\\site\\wwwroot";

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static bool IsDevelopmentEnvironment()
        {
            return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")?
                .Equals("Development", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}