using BookStore.Data.DTOs.Author;
using BookStore.Data.Seeder;
using BookStore.Services;
using BookStore.Services.Interfaces;
using BookStore.Validators.Author;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookStoreData(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));
        services.AddScoped<DataSeeder>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateAuthorDtoValidator>();
        
        services.MapInterfaceToServices();
        
        return services;
    }

    private static void MapInterfaceToServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorService, AuthorService>();
    }
}