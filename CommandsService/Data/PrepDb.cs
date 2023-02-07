using CommandsService.Entities;
using CommandsService.Interfaces;
using Serilog;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var grpcClient = serviceScope.ServiceProvider.GetService<IDataClient<Platform>>();
        var platforms = grpcClient?.ReturnAll();

        if (platforms != null)
        {
            SeedData(serviceScope.ServiceProvider.GetService<IRepository<Platform>>(), platforms);
        }
    }

    private static void SeedData(IRepository<Platform>? platformsRepository, IEnumerable<Platform> platforms)
    {
        if (platformsRepository == null)
        {
            throw new InvalidOperationException();
        }
        
        Log.Information("Seeding new platforms...");

        foreach (var platform in platforms)
        {
            if (!platformsRepository.Exists(x => x.ExternalId == platform.ExternalId))
            {
                platformsRepository.Create(platform);
            }
        }

        platformsRepository.SaveChanges();
    }
}