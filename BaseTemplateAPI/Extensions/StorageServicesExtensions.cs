using BaseTemplateAPI.Data;
using BaseTemplateAPI.Entity.Table;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Text;

namespace BaseTemplateAPI.Extensions
{
	public static class StorageServicesExtensions
	{
		public static IServiceCollection StorageServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DataContext>(x =>
			{
				x.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds)
				);
			});
			var builder = services.AddIdentityCore<AppUser>();
			builder = new IdentityBuilder(builder.UserType, builder.RoleType, builder.Services);
			builder.AddRoles<IdentityRole>();
			builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
			builder.AddSignInManager<SignInManager<AppUser>>();

			services.AddSingleton<IConnectionMultiplexer>(c =>
			{
				var redisConfiguration = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
				return ConnectionMultiplexer.Connect(redisConfiguration);
			});

			return services;
		}
	}
}
