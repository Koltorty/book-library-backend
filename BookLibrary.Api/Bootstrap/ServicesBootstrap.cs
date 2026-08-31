using BookLibrary.Api.Services;

namespace BookLibrary.Api.Bootstrap;

public static class ServicesBootstrap
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<BookService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<PublisherService>();
        services.AddScoped<SeriesService>();
    }
}