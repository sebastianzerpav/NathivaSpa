using AppWebSpa.Data;
using AppWebSpa.Services;
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
            
            return builder;
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISpaServicesService, SpaServicesService>();
        }

    }
}
