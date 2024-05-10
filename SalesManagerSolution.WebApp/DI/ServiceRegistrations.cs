using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.Interfaces.Authentications;
using SalesManagerSolution.Core.Interfaces.Common;
using SalesManagerSolution.Core.Interfaces.Orders;
using SalesManagerSolution.Core.Interfaces.Services.Carts;
using SalesManagerSolution.Core.Interfaces.Services.Categories;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.Services.Authentications;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.HttpClient;
using SalesManagerSolution.Infrastructure.EntityFramework;
using SalesManagerSolution.Infrastructure.Services.Authentications;
using SalesManagerSolution.Infrastructure.Services.Carts;
using SalesManagerSolution.Infrastructure.Services.Common;
using SalesManagerSolution.Infrastructure.Services.Orders;
using SalesManagerSolution.Infrastructure.Services.Products;

namespace SalesManagerSolution.WebApp.DI
{
	public static class ServiceRegistrations
	{
		public static void ConfigureAppServices(this IServiceCollection services, ConfigurationManager configuration, bool IsProduction)
		{
			var connectionString = configuration.GetConnectionString(SystemConstants.MainConnectionString);
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Stage")
			{
				services.AddDbContext<ApplicationDbContext>(options =>
							options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"), x => x.MigrationsAssembly("SalesManagerSolution.Database")));
			}
			else
			{
				services.AddDbContext<ApplicationDbContext>(options =>
							options.UseSqlServer(connectionString));
			}

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			// Add services to the container.
			services.AddControllersWithViews();

			services.AddIdentity<AppUser, AppRole>()
						.AddEntityFrameworkStores<ApplicationDbContext>()
						.AddDefaultTokenProviders();

			services.AddHttpClient();
			services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionsName));
			services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
			services.AddScoped<IStorageService, FileStorageService>();
			services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IProductApiClient,ProductApiClient>();
			services.AddScoped<ICategoryApiClient, CategoryApiClient>();
			services.AddScoped<ICartApiClient, CartApiClient>();
		}
	}
}
