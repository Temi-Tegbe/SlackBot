namespace SlackAPI.Models
{
    public class Settings
    {
        public string SlackAccessToken { get; set; }
        public string SlackSigningSecret { get; set; }
        public string BalanceRecipientUserId { get; set; }
        public string VanServiceUrl { get; set; }
        public string AccountBalanceUrl { get; set; }
        public string ServiceMonitoringChannelId { get; set; }
        public string BalanceEnquirySchedule { get; set; }
        public string VanServicePingSchedule { get; set; } 
    }
}
