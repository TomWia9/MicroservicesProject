using Microsoft.EntityFrameworkCore;
using PlatformService.Entities;
using Serilog;

namespace PlatformService.Data;

public static class DatabaseSeed
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException(),
            isProduction);
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Log.Information("Attempting to apply migrations");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Error("Could not run migrations: {EMessage}", e.Message);
            }
        }
        
        if (context.Platforms.Any()) return;
        
        Log.Information("Seeding database");

        context.Platforms.AddRange(
            new Platform
            {
                Name = ".NET",
                Publisher = "Microsoft",
                Cost = "Free"
            },
            new Platform
            {
                Name = "SQL Server Express",
                Publisher = "Microsoft",
                Cost = "Free"
            },
            new Platform
            {
                Name = "Kubernetes",
                Publisher = "Cloud Native Computing Foundation",
                Cost = "Free"
            }
        );

        context.SaveChanges();
    }
}