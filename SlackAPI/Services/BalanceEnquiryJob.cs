namespace SlackAPI.Services;

public class BalanceEnquiryJob
{
        private readonly ILogger<MonitorServicesJob> _logger;
        private readonly BalanceEnquiryPing _balanceEnquiryPing;
        public static readonly string JobName = nameof(BalanceEnquiryJob);

        public BalanceEnquiryJob(BalanceEnquiryPing balanceEnquiryPing)
        {
            _balanceEnquiryPing = balanceEnquiryPing;
        }

        public async Task Run()
        {
            var vanService = await _balanceEnquiryPing.GetAccountBalance();
        }

    }