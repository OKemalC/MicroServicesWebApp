using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Azure.ServiceBus;
using QueueReceiverService.Models;
using QueueReceiverService.Services;
using System.Text;
using System.Text.Json;

namespace QueueReceiverService
{
    public class ReceivingSQSWorker : BackgroundService
    { 
        private readonly IConfiguration _config; 
        private readonly IQueueSQSService _queueSQSService;
     
        public ReceivingSQSWorker(IConfiguration config, IQueueSQSService queueSQSService)
        {
            _config = config;
            _queueSQSService = queueSQSService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queueSQSService.ReceiveSQSMessages(); 

                await Task.Delay(5000, stoppingToken);
           
            }
        }
    
    }
}
