using BaseTemplateAPI.Errors;
using BaseTemplateAPI.Interfaces;
using BaseTemplateAPI.Services;
using CEOWB.SmsGateway.Client;
using CEOWB.SmsGateway.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ISmsGatewayClient>(x => new SmsGatewayClient(configuration.GetValue<string>("MessageSettings:SmsGatewayEndpoint"), configuration.GetValue<string>("MessageSettings:APIKey"), new HttpClient()));

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IOtpRepository, OtpRepository>();

            services.AddScoped<ILogicalErrorMessage, LogicalErrorMessage>();

            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = actionContext => {
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

            return services;
        }
    }
}
