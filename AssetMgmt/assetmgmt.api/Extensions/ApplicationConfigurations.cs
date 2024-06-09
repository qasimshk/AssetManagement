using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using assetmgmt.core.Common.Results;
using assetmgmt.api.Middleware;
using assetmgmt.core.Validators;

namespace assetmgmt.api.Extensions
{
    public static class ApplicationConfigurations
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
            .ConfigureApiBehaviorOptions(opt => opt.InvalidModelStateResponseFactory = context =>
            {
                var errors = new List<string>();

                foreach (var fields in context.ModelState.Keys)
                {
                    errors.AddRange(from error in context.ModelState[fields]?.Errors
                                    select error.ErrorMessage);
                }

                return new BadRequestObjectResult(Result.FailedResult(errors, HttpStatusCode.BadRequest));
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            // Middlewares
            services.AddTransient<ExceptionHandlingMiddleware>();

            // Fluent Validator
            services.AddValidatorsFromAssemblyContaining<CreateAssetRequestValidator>();

            return services;
        }
    }
}
