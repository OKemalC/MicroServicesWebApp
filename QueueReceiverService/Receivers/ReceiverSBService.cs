using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using QueueReceiverService.Models;
using QueueReceiverService.Services;
using System.Text;

namespace QueueReceiverService.Receivers
{
    public class ReceiverSBService : IQueueSBService
    {
        private readonly IConfiguration _config;
        static ServiceBusClientOptions? clientOptions;
        static ServiceBusClient? sbClient;
        static ServiceBusProcessor? processor;
        public ReceiverSBService(IConfiguration config)
        {
            _config = config; 

            clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            sbClient = new ServiceBusClient(_config.GetConnectionString("AzureSBEndpoint"), clientOptions);
            processor = sbClient.CreateProcessor(_config.GetConnectionString("AzureSBQueueName"), new ServiceBusProcessorOptions());

        }

        public async Task ReceiveSBMessages()
        {
            SubscriptionClient subscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureSBEndpoint"), "SubServiceBus", _config.GetConnectionString("AzureSBQueueName"));
            try
            {
                subscriptionClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        var messageJson = Encoding.UTF8.GetString(message.Body);
                        string tryplss = messageJson.ToString();
                        //var updateMessage = JsonConvert.DeserializeObject<QueueMessage>(messageJson);

                        Console.WriteLine($"Received message with productId: {tryplss}");

                        await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    },
                    new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                    { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            //try
            //{ 
            //    processor.ProcessMessageAsync += MessageHandler;

            //    processor.ProcessErrorAsync += ErrorHandler;

            //    await processor.StartProcessingAsync();
            //    Console.WriteLine("Wait for a minute and then press any key to end the processing");

            //    await processor.StopProcessingAsync(); 
            //}
            //finally
            //{
            //    await processor.DisposeAsync();
            //    await sbClient.DisposeAsync();
            //}

        }
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
