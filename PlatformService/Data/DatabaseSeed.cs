using PlatformService.Entities;
using Serilog;

namespace PlatformService.Data;

public static class DatabaseSeed
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException());
    }

    private static void SeedData(AppDbContext context)
    {
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