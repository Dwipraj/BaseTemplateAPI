using BaseTemplateAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BaseTemplateAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			try
			{
				Log.Information("Application starting up.");
				var host = CreateHostBuilder(args).Build();

				using (var scope = host.Services.CreateScope())
				{
					var services = scope.ServiceProvider;
					var config = services.GetRequiredService<IConfiguration>();
					var context = services.GetRequiredService<DataContext>();
					try
					{
						await context.Database.MigrateAsync();
					}
					catch (Exception ex)
					{
						Log.Error(ex, "An error occured during migration");
					}
					try
					{
						//var isDev = services.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
						//var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
						//var userManager = services.GetRequiredService<UserManager<AppUser>>();
						//await DataContextSeed.SeedAsync(context, isDev, userManager, roleManager);
					}
					catch (Exception ex)
					{
						Log.Error(ex, "An error occured during seeding");
					}
				}

				host.Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application failed to start.");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
