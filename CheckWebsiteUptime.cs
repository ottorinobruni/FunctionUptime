using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionUptime;

public class CheckWebsiteUptime
{
    private readonly ILogger _logger;
    private static readonly HttpClient _httpClient = new HttpClient();

    public CheckWebsiteUptime(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CheckWebsiteUptime>();
    }

    [Function("CheckWebsiteUptime")]
    public async Task RunAsync([TimerTrigger("%TimerSchedule%")] TimerInfo myTimer)
    {
        var urlToCheck = Environment.GetEnvironmentVariable("UrlToCheck");
        var logicAppUrl = Environment.GetEnvironmentVariable("LogicAppUrl");
        
        try
        {
            var response = await _httpClient.GetAsync(urlToCheck);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Website is not reachable. Status code: {response.StatusCode}");

                var payload = new
                {
                    url = urlToCheck,
                    status = "fail",
                    statusCode = (int)response.StatusCode,
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await _httpClient.PostAsync(logicAppUrl, content);
            }
            else
            {
                _logger.LogInformation($"Website responded successfully at {DateTime.Now}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking website: {ex.Message}");
            
            var errorPayload = new
            {
                url = urlToCheck,
                status = "error",
                statusCode = 0,
                timestamp = DateTime.UtcNow.ToString("o")
            };
            var errorJson = JsonSerializer.Serialize(errorPayload);
            var errorContent = new StringContent(errorJson, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(logicAppUrl, errorContent);
        }
    }
}