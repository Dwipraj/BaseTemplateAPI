using BaseTemplateAPI.Errors;
using BaseTemplateAPI.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace BaseTemplateAPI.Extensions
{
	public static class RequestResponseServicesExtensions
	{
		public static IServiceCollection RequestResponseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(MappingProfiles));
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.RequireHttpsMetadata = false;
				opt.SaveToken = true;
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
					ValidIssuer = configuration["Token:Issuer"],
					ValidateIssuer = true,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
			services.AddAuthorization();
			services.AddControllers().AddFluentValidation(cfg =>
			{
				cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
			}).AddJsonOptions(opts =>
			{
				opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = actionContext =>
				{
					var errors = actionContext.ModelState
					.Where(e => e.Value.Errors.Count > 0)
					.SelectMany(x => x.Value.Errors)
					.Select(x => x.ErrorMessage)
					.ToArray();

					var errorResponse = new ApiValidationErrorResponse
					{
						Errors = errors
					};

					return new BadRequestObjectResult(errorResponse);
				};
			});

			services.AddCors(opt =>
			{
				opt.AddPolicy("CorsPolicy", policy =>
				{
					policy
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin()
					.WithExposedHeaders("WWW-Authenticate");
				});
			});

			return services;
		}
	}
}
