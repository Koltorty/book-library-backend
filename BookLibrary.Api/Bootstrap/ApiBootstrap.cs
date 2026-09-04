using System.Text.Json;
using System.Text.Json.Serialization;
using BookLibrary.Api.ExceptionHandling;
using BookLibrary.Api.Serialization;
using FluentValidation;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace BookLibrary.Api.Bootstrap;

public static class ApiBootstrap
{
    public static void AddApiServices(this IServiceCollection services)
    {
        AddControllers(services);
        AddCors(services);
        AddExceptionHandling(services);
        AddValidators(services);
        AddSwagger(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BookLibrary API",
                Description = "API для управления библиотекой книг"
            });
        });
    }

    private static void AddExceptionHandling(IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
            });
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
