using AppWebSpa.Data;
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
            
            return builder;
        }
    }
}
