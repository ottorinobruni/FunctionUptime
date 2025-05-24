using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionUptime;

public class CheckWebsiteUptime
{
    private readonly ILogger _logger;

    public CheckWebsiteUptime(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CheckWebsiteUptime>();
    }

    [Function("CheckWebsiteUptime")]
    public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        var urlToCheck = "https://www.ottorinobruni.com";
        var logicAppUrl = "https://prod-119.westeurope.logic.azure.com:443/workflows/c449d5e3238a4ecfbcb24a77ac65cb92/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=076zeZ0-MP-aGBvC95IVZ3JXXi7DNN-nNaaXj9mnT7k";
        var httpClient = new HttpClient();

        try
        {
            var response = await httpClient.GetAsync(urlToCheck);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Website is not reachable. Status code: {response.StatusCode}");

                await httpClient.PostAsync(logicAppUrl, null);
            }
            else
            {
                _logger.LogInformation($"Website responded successfully at {DateTime.Now}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking website: {ex.Message}");

            await httpClient.PostAsync(logicAppUrl, null);
        }
    }
}