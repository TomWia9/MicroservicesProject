using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using PlatformService.Interfaces;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpCommandDataClient> _logger;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, ILogger<HttpCommandDataClient> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformDto platform)
    {
        var httpContent =
            new StringContent(JsonSerializer.Serialize(platform), Encoding.UTF8, "application/json");
        var response =
            await _httpClient.PostAsync($"{_configuration["Urls:CommandService"]}", httpContent);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Sync POST to CommandsService was ok!");
        }
        else
        {
            _logger.LogError("Sync POST to CommandsService was not OK!");
        }
    }
}