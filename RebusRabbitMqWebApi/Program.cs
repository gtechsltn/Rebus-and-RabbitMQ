using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.RabbitMq;
using Rebus.ServiceProvider;
using Rebus.Routing.TypeBased;
using RebusRabbitMqWebApi.Handlers;
using RebusRabbitMqWebApi.Models;


var builder = WebApplication.CreateBuilder(args);


// Replace with the actual connection string for your remote RabbitMQ server
string rabbitMqConnectionString = "amqp://guest:guest@127.0.0.1:5672/";

//string rabbitMqConnectionString = "amqp://username:password@remote-server-hostname:port/vhost";

// Register Rebus with the services
builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq(rabbitMqConnectionString, "example-queue"))
    .Routing(r => r.TypeBased().MapAssemblyOf<MyMessage>("example-queue")) // Adjust YourMessageType
    .Options(o => o.SetNumberOfWorkers(1)),
    isDefaultBus: true
);

builder.Services.AddControllers();


builder.Services.AutoRegisterHandlersFromAssemblyOf<MyMessageHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.Lifetime.ApplicationStarted.Register(() =>
{
    //var bus = app.Services.GetRequiredService<IBus>();
    // Now use bus here safely, e.g., send a startup message

// Start and stop Rebus with the app lifecycle
var bus = app.Services.GetRequiredService<IBus>();

app.Lifetime.ApplicationStarted.Register(() => bus.Advanced.Workers.SetNumberOfWorkers(1));
app.Lifetime.ApplicationStopping.Register(() => bus.Advanced.Workers.SetNumberOfWorkers(0));

    //app.Services.UseRebus(); // Initializes the Rebus bus and starts processing
});

app.MapControllers();


app.Run(); // Run the web application

