using MassTransit;
using Tracing.Shared.Messages;

namespace Tracing.WorkerService
{
    public class SubmitOrderConsumer : IConsumer<OrderMessage>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }


        public Task Consume(ConsumeContext<OrderMessage> context)
        {
            _logger.LogInformation("Request received.");

            return Task.CompletedTask;
        }
    }
}
