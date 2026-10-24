using BookStore.Data;
using BookStore.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Services.AddBookStoreData(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

using var host = builder.Build();
using var scope = host.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

var command = args.FirstOrDefault();

try
{
    switch (command)
    {
        case "migrate":
            Console.WriteLine("Applying migrations...");
            await db.Database.MigrateAsync();
            break;

        case "seed":
            Console.WriteLine("Seeding data...");
            await seeder.SeedAsync(CancellationToken.None);
            break;

        case "fresh":
            Console.WriteLine("Dropping database...");
            await db.Database.EnsureDeletedAsync();
            Console.WriteLine("Applying migrations...");
            await db.Database.MigrateAsync();
            if (args.Contains("--seed"))
            {
                Console.WriteLine("Seeding data...");
                await seeder.SeedAsync(CancellationToken.None);
            }
            break;

        default:
            Console.WriteLine("Usage: migrate | seed | fresh [--seed]");
            return 1;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed: {ex.Message}");
    return 1;
}

Console.WriteLine("Done.");
return 0;