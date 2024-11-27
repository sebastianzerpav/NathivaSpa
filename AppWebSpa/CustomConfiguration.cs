using AppWebSpa.Data;
using AppWebSpa.Data.Entities;
using AppWebSpa.Data.Seeders;
using AppWebSpa.Helpers;
using AppWebSpa.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AppWebSpa
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            //DataContext 
            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));

            }
            );

            //servicio especial, valida si los usuarios tienen permiso para cierta funcion
            builder.Services.AddHttpContextAccessor();

            //services
            AddServices(builder);

            // IAM: Identity and Access Management- sistema de autentiticacion y gestion de identidad
            AddIAM(builder);

            //PAM: Privileged access Management

            //Toast Notification
            builder.Services.AddNotyf(config => 
            { 
                config.DurationInSeconds = 10; 
                config.IsDismissable = true; 
                config.Position = NotyfPosition.BottomRight; 
            });


            return builder;
        }
        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                //Seguridad de la contraseña
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            //configuracion de Tokens o cookies
            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                //En cuanto expira la Cookie
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                //Donde dirigir cuando expira la cookie
                conf.LoginPath = "/Account/Login";
                //Ruta para acceso denegado
                conf.AccessDeniedPath = "/Account/NotAuthorized";
            });
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            //Services
            builder.Services.AddScoped<ISpaServicesService, SpaServicesService>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IHomeService, HomeService>();


            //SeedDb
            builder.Services.AddTransient<SeedDb>();

            //Helpers
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();

        }

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);
            return app;

        }

        //INYECCION DEL SEEDER- FORMA DE INYECCION EN EL PROGRAM
        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();
            using (IServiceScope scope = scopeFactory!.CreateScope()) 
            { 
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }
        }
    }
}
