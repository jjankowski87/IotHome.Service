using System;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using IotHome.Service.App.Configuration;
using IotHome.Service.App.Services;
using IotHome.Service.App.Services.Interfaces;
using IotHome.Service.App.Utils;
using IotHome.Service.Model;
using IotHome.Service.Services;
using IotHome.Service.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IotHome.Service.App
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(o =>
            {
                if (_env.IsDevelopment())
                {
                    o.DetailedErrors = true;
                }
            });

            services.AddSingleton(GetConfiguration<StorageConfiguration>("StorageConfiguration"));
            services.AddSingleton(GetConfiguration<RawAppConfiguration>("AppConfiguration"));
            services.AddSingleton<AppConfiguration>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddSingleton<IReadingsNotifier, ReadingsNotifier>();
            services.AddScoped<IStorageExplorer, StorageExplorer>();
            services.AddScoped<IReadingsService, ReadingsService>();
            services.AddScoped<IStorageHelper, StorageHelper>();

            services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true; // optional
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.ApplicationServices
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }

        private TConfiguration GetConfiguration<TConfiguration>(string sectionName)
        {
            var config = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration));
            Configuration.GetSection(sectionName).Bind(config);

            return config;
        }
    }
}
