using AutoMapper;
using CommandsService.Entities;
using CommandsService.Interfaces;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc;

public class DataClient<T> : IDataClient<T> where T: EntityBase
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ILogger<DataClient<T>> _logger;

    public DataClient(IConfiguration configuration, IMapper mapper, ILogger<DataClient<T>> logger)
    {
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;
    }
    
    public IEnumerable<T>? ReturnAll()
    {
        _logger.LogInformation("Calling GRPC Service {GrpcPlatform}", _configuration["GrpcPlatform"]);

        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"] ?? throw new InvalidOperationException());
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request); //TODO change to generic GetAll()
            return _mapper.Map<IEnumerable<T>>(reply.Platform);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not call GRPC server {E}", e.Message);
            return null;
        }
    }
}