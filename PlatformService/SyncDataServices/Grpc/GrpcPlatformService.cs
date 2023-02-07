using AutoMapper;
using Grpc.Core;
using PlatformService.Entities;
using PlatformService.Interfaces;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IRepository<Platform> _platformsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcPlatformService> _logger;

    public GrpcPlatformService(IRepository<Platform> platformsRepository,
        IMapper mapper,
        ILogger<GrpcPlatformService> logger)
    {
        _platformsRepository = platformsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _platformsRepository.GetAll();

        foreach (var platform in platforms)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));  
        }

        return Task.FromResult(response);
    }
}