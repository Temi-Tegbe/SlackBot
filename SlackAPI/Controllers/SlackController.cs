using Microsoft.AspNetCore.Mvc;
using SlackAPI.Models;
using SlackAPI.Services;
using SlackNet;
using SlackNet.AspNetCore;

namespace SlackAPI.Controllers
{
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ISlackRequestHandler _requestHandler;
        private readonly SlackEndpointConfiguration _endpointConfig;
        private readonly ISlackApiClient _slack;
        private readonly VanServicePing _vanServicePing;
        public SlackController(ISlackRequestHandler requestHandler, SlackEndpointConfiguration endpointConfig, ISlackApiClient slack, VanServicePing vanServicePing)
        {
            _requestHandler = requestHandler;
            _endpointConfig = endpointConfig;
            _slack = slack;
            _vanServicePing = vanServicePing;
        }

        [HttpPost]
        [Route("[Controller]/PingVanService")]
        public async Task<ActionResult> Submit()
        {
            var response = _vanServicePing.PingVanService();
            return Ok();
        }

        [HttpPost]
        [Route("[Controller]/Event")]
        public async Task<IActionResult> Event()
        {
             return await _requestHandler.HandleEventRequest(HttpContext.Request, _endpointConfig);
        }
        
        [HttpPost]
        [Route("[Controller]/Submit")]
        public async Task<ActionResult> Submit([FromBody] SlackRequest request)
        {
            await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = request.Message, Channel = request.SlackChannel }, null);
            return Ok();
        }

    }
}