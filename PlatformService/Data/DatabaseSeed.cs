using PlatformService.Entities;

namespace PlatformService.Data;

public static class DatabaseSeed
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    private static void SeedData(AppDbContext context)
    {
        if(!context.Platforms.Any())
        {
            System.Console.WriteLine("Seeding data...");

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
                    Publisher = "Cloud Native Computing Fundation",
                    Cost = "Free"
                }
            );

            context.SaveChanges();
        }
    }
}