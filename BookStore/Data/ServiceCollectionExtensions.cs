using BookStore.Data.Seeder;
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
}