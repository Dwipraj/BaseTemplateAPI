using BaseTemplateAPI.Interfaces;
using BaseTemplateAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseTemplateAPI.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddScoped<ITokenService, TokenService>();

			services.AddScoped<IOtpRepository, OtpRepository>();

			services.AddScoped<ILogicalErrorMessage, LogicalErrorMessage>();

			return services;
		}
	}
}
