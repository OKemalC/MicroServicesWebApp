using QueueReceiverService.Services;

namespace QueueReceiverService
{
    public class ReceivingWorker: BackgroundService
    {
        private readonly IConfiguration _config; 
        private readonly IQueueReceiverService _queueService;
         
        public ReceivingWorker(IConfiguration config, IQueueReceiverService queueService)
        {
            _config = config; 
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                await _queueService.ReceiveMessageAsync();

                await Task.Delay(5000, stoppingToken); 
            }
        }  

    }
}
