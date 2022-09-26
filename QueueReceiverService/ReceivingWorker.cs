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
    public class ReceivingWorker : BackgroundService
    { 
        private readonly IConfiguration _config;
        static IQueueClient queueClient;
        private readonly IQueueReceiverService _queueReceiverService;
     
        public ReceivingWorker( IConfiguration config,IQueueReceiverService queueReceiverService)
        {
            _config = config;

            queueClient = new QueueClient(_config.GetConnectionString("AzureSBEndpoint"), _config.GetConnectionString("AzureSBQueueName"));
             _queueReceiverService= queueReceiverService;
        }
    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queueReceiverService.ReceiveSQSMessages(); 
                await _queueReceiverService.ReceiveSBMessages();


                await Task.Delay(5000, stoppingToken);
           
            }
        }
   

        //private async Task<QueueMessage> ReceiveSBAsync()
        //{  
        //    var messageandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        //    {
        //        MaxConcurrentCalls = 1,
        //        AutoComplete = false
        //    };
        //    queueClient.RegisterMessageHandler(ProcessMessageAsync, messageandlerOptions);
              

        //    await queueClient.CloseAsync();
        //    return null;
        //}
        //private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        //{
        //    Console.WriteLine($"Error: {arg.Exception}");
        //    return Task.CompletedTask;
        //}
        //private static async Task ProcessMessageAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        //{ 
        //    var jsonString = Encoding.UTF8.GetString(message.Body);
        //    QueueMessage sbMessage = JsonSerializer.Deserialize<QueueMessage>(jsonString);  
        //    await queueClient.CompleteAsync(message.SystemProperties.LockToken); 
        //}
    }
}
