using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using SlackAPI.Handlers;
using SlackAPI.Models;
using SlackAPI.Services;
using SlackNet.AspNetCore;
using SlackNet.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<VanServicePing>();
builder.Services.AddSingleton<BalanceEnquiryPing>();
builder.Services.AddHttpClient();
builder.Services.AddHangfire(config => config.UseMemoryStorage());

builder.Services.AddHangfireServer();
builder.Services.AddHostedService<RegisterRecurringJobs>();

var settings = builder.Configuration.GetSection("Slack").Get<Settings>();

var accessToken = Environment.GetEnvironmentVariable("SlackAccessToken") ?? settings.SlackAccessToken; 
var signingSecret = Environment.GetEnvironmentVariable("SlackSigningSecret") ?? settings.SlackSigningSecret;
builder.Services.AddSingleton(settings);

#if DEBUG
builder.Services.AddSingleton(new SlackEndpointConfiguration());
#else
builder.Services.AddSingleton(new SlackEndpointConfiguration().UseSigningSecret(signingSecret));
#endif

builder.Services.AddSlackNet(c => c
.UseApiToken(accessToken)
.RegisterEventHandler<MessageEvent, PingHandler>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHangfireDashboard();
app.Run();

