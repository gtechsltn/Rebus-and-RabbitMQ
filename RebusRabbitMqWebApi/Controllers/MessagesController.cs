using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using RebusRabbitMqWebApi.Models;

namespace RebusRabbitMqWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IBus _bus;

        public MessagesController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MyMessage message)
        {
            await _bus.Send(message);
            return Ok("Message sent");
        }
    }
}
