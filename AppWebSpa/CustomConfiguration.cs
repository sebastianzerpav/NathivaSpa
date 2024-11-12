using AppWebSpa.Data;
using AppWebSpa.Helpers;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AppWebSpa
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));

            }
            );

            //services
            AddServices(builder);

            //Toast Notification
            builder.Services.AddNotyf(config => 
            { 
                config.DurationInSeconds = 10; 
                config.IsDismissable = true; 
                config.Position = NotyfPosition.BottomRight; 
            });


            return builder;
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            //Services
            builder.Services.AddScoped<ISpaServicesService, SpaServicesService>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();

            //Helpers
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
        }

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();
            return app;

        }
    }
}
