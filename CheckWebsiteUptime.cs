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
    public async Task RunAsync([TimerTrigger("0 0 */12 * * *")] TimerInfo myTimer)
    {
        var urlToCheck = "YOUR-SITE";
        var logicAppUrl = "YOUR-LOGIC-APP-URL";
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