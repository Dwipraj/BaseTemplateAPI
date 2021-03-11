using BaseTemplateAPI.Data;
using BaseTemplateAPI.Entity.Table;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
			ILogger logger = host.Services.GetService<ILogger<Program>>();
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var isDev = services.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
				var config = services.GetRequiredService<IConfiguration>();
				try
				{
					var context = services.GetRequiredService<DataContext>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					var userManager = services.GetRequiredService<UserManager<AppUser>>();
					await context.Database.MigrateAsync();
					//await DataContextSeed.SeedAsync(context, userManager, roleManager);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "An error occured during migration or seeding");
				}
			}
			host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
