using SlackAPI.Models;
using SlackNet;

namespace SlackAPI.Services;

public class BalanceEnquiryPing
{
    private readonly ISlackApiClient _slack;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string mediaType = "application/json";
    private readonly Settings _appSettings;

    public BalanceEnquiryPing(ISlackApiClient slack, IHttpClientFactory httpClientFactory, Settings appSettings)
    {
        _slack = slack;
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings;
    }

    public async Task<bool> GetAccountBalance()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = new Uri(_appSettings.AccountBalanceUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization",
                "Bearer xyz");
            var apiCall = await client.GetAsync(url);
            var response = await apiCall.Content.ReadAsStringAsync();
            if (!apiCall.IsSuccessStatusCode)
            {
                await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = $"Could not fetch Wema balance. StatusCode: {apiCall.StatusCode}, Response: {response}", Channel = _appSettings.ServiceMonitoringChannelId }, null);
            }
            var deserialisedResponse = new BalanceEnquiryResponse()
            {
                AccountBalance = response
            };
            var amountInDecimal = Decimal.Parse(deserialisedResponse.AccountBalance);
            amountInDecimal = Math.Round(amountInDecimal, 2);
            await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = $"Wema Bank account balance as at {DateTime.Now}: \u20a6\n{amountInDecimal:N} ", Channel = _appSettings.BalanceRecipientUserId }, null);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}