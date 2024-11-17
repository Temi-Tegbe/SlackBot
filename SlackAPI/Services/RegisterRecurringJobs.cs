using Hangfire;
using SlackAPI.Models;

namespace SlackAPI.Services;

public class RegisterRecurringJobs: IHostedService
{
    private readonly Settings _appSettings;

    public RegisterRecurringJobs(Settings appSettings)
    {
        _appSettings = appSettings;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var connected = false;
        while (!connected)
        {
            //only proceed when hangfire is ready
            connected = HangfireIsInitialized();
        }

        RecurringJob.AddOrUpdate<MonitorServicesJob>(MonitorServicesJob.JobName,
            methodCall: x => x.Run(), _appSettings.VanServicePingSchedule);
        RecurringJob.AddOrUpdate<BalanceEnquiryJob>(BalanceEnquiryJob.JobName,
            methodCall: x => x.Run(), _appSettings.BalanceEnquirySchedule);
        
        return Task.CompletedTask;
    }
    
    private static bool HangfireIsInitialized()
    {
        try
        {
            return JobStorage.Current is not null;
        }
        catch
        {
            return false;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}