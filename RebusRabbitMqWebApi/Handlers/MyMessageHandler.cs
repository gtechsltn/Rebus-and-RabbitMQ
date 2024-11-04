using Rebus.Handlers;
using RebusRabbitMqWebApi.Models;

namespace RebusRabbitMqWebApi.Handlers
{
    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message)
        {
            Console.WriteLine($"Received message: {message.Content}");
            return Task.CompletedTask;
        }
    }
}
