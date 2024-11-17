namespace SlackAPI.Services;

public class MonitorServicesJob
{
    private readonly ILogger<MonitorServicesJob> _logger;
    private readonly VanServicePing _vanServicePing;
    public static readonly string JobName = nameof(MonitorServicesJob);

    public MonitorServicesJob(VanServicePing vanServicePing)
    {
        _vanServicePing = vanServicePing;
    }

    public async Task Run()
    {
        var vanService = await _vanServicePing.PingVanService();
    }

}