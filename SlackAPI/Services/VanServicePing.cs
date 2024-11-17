using System.Text;
using Newtonsoft.Json;
using SlackAPI.Models;
using SlackNet;

namespace SlackAPI.Services;

public class VanServicePing
{
    private readonly ISlackApiClient _slack;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string mediaType = "application/json";
    private readonly Settings _appsettings;

    public VanServicePing(ISlackApiClient slack, IHttpClientFactory httpClientFactory, Settings appsettings)
    {
        _slack = slack;
        _httpClientFactory = httpClientFactory;
        _appsettings = appsettings;
    }

    public async Task<bool> PingVanService()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var request = new VanServicePingRequest()
            {
                AccountNumber = "",
                BankCode = ""
            };
            var url = new Uri(_appsettings.VanServiceUrl);
            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, mediaType);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization",
                "Bearer xyz");
            var apiCall = await client.PostAsync(url, payload);
            var response = await apiCall.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<VanServicePingResponse>(response);
            if (!apiCall.IsSuccessStatusCode || deserialisedResponse?.Success == false)
            {
                await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = $"Could not reach VAN Service. Status Code: {apiCall.StatusCode}. Response: {response}", Channel = _appsettings.ServiceMonitoringChannelId }, null);
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}