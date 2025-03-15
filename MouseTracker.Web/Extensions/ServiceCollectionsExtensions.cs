using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MouseTracker.Domain.Data;
using Newtonsoft.Json.Serialization;
using MouseTracker.Application.Validators;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using MouseTracker.Application.Models;
using MouseTracker.Web.Middlewares;
using MouseTracker.Application.Abstractions;
using MouseTracker.Infrastructure.Services;

namespace MouseTracker.Web.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(opt =>
                opt.UseInMemoryDatabase("MouseTrackerDB"));
            return builder;
        }

        public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MouseTracker API",
                    Version = "v1"
                });
            });

            return builder;
        }

        public static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddFluentValidationRulesToSwagger();
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            builder.Services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });
            return builder;
        }

        public static WebApplicationBuilder AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new AutoMapperProfile()));
            return builder;
        }

        public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPositionsService, PositionService>();
            return builder;
        }

        public static WebApplicationBuilder AddExceptionHandler(this WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            return builder;
        }
    }
}
