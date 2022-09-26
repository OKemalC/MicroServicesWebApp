using QueueReceiverService.Services;

namespace QueueReceiverService
{
    public class ReceivingSBWorker : BackgroundService
    {
        private readonly IConfiguration _config; 
        private readonly IQueueSBService _queueSBService;


        public ReceivingSBWorker(IConfiguration config, IQueueSBService queueSBService)
        {
            _config = config;
            _queueSBService = queueSBService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queueSBService.ReceiveSBMessages();

                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
