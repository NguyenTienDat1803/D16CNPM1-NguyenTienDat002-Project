using Microsoft.AspNetCore.Authentication.Cookies;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.Interfaces.Authentications;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.HttpClient.System.User;
using SalesManagerSolution.HttpClient;
using SalesManagerSolution.HttpClient.System.Roles;

namespace SalesManagerSolution.AdminApp.DependencyInjections
{
    public static class ServiceRegistrations
    {

        public static void ConfigureAppServices(this IServiceCollection services, ConfigurationManager configuration, bool IsProduction)
        {
            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/User/Forbidden/";
                });


            // Add services to the container.
            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICategoryApiClient, CategoryApiClient>();
            services.AddScoped<IUserHttpClient, UserHttpClient>();
			services.AddScoped<IRoleApiClient, RoleApiClient>();
			services.AddScoped<IProductApiClient, ProductApiClient>();
            services.AddScoped<IOrderApiClient, OrderApiClient>();
        }
    }
}
