namespace SlackAPI.Models;

public record VanServicePingRequest
{
    public string AccountNumber { get; set; }
    public string BankCode { get; set; }
}

public record VanServicePingResponse
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public string? Data { get; set; }
}