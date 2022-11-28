using MassTransit;
using Tracing.Shared.Messages;
using Tracing.Shared.TraceAndLong;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tracingAndLogSection = configuration.GetSection("TracingAndLogging");

builder.Services.ConfigureTracing(tracingAndLogSection);

var rabbitMqConfig = new RabbitMqConfiguration();
configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConfig.HostName, "/", h =>
        {
            h.Username(rabbitMqConfig.UserName);
            h.Password(rabbitMqConfig.Password);
        });
    });
});

builder.Host.ConfigureLogging(logging => logging.ClearProviders().ConfigureLogging(tracingAndLogSection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
