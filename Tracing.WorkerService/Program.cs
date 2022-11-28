using MassTransit;
using Tracing.Shared.Messages;
using Tracing.Shared.TraceAndLong;
using Tracing.WorkerService;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        //services.AddHostedService<Worker>();

        var tracingAndLogSection = context.Configuration.GetSection("TracingAndLogging");

        services.ConfigureTracing(tracingAndLogSection);

        var rabbitMqConfig = new RabbitMqConfiguration();
        context.Configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig.HostName, "/", h =>
                {
                    h.Username(rabbitMqConfig.UserName);
                    h.Password(rabbitMqConfig.Password);
                });

                cfg.ReceiveEndpoint(OrderMessage.EndPointAddress,  xr =>
                {
                    xr.ConfigureConsumer<SubmitOrderConsumer>(context);
                });
            });
        });
    })
    .ConfigureLogging((context, x) =>
    {
        var tracingAndLogSection = context.Configuration.GetSection("TracingAndLogging");

        x.ClearProviders().ConfigureLogging(tracingAndLogSection);
    })
    .Build();

await host.RunAsync();
