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
        static IQueueClient queueClient;
        QueueMessage SBMessage = new QueueMessage();
        string UUUUMessage = "";
        public ReceiverSBService(IConfiguration config)
        {
            _config = config; 
             
            queueClient= new QueueClient(_config.GetConnectionString("AzureSBEndpoint"), _config.GetConnectionString("AzureSBQueueName"));
            var messageandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
        }

        public async Task<string> ReceiveSBMessages()
        {
            try
            {
                queueClient.RegisterMessageHandler(
                     async (message, token) =>
                     {
                         var messageJson = Encoding.UTF8.GetString(message.Body);
                         string tryplss = messageJson.ToString();
                         UUUUMessage = messageJson.ToString();
                         //SBMessage.ServiceBusMessage= messageJson.
                         //SBMessage = JsonConvert.DeserializeObject<QueueMessage>(messageJson);

                         Console.WriteLine($"Received message with productId: {tryplss}");

                         await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                     },
                     new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
                     { MaxConcurrentCalls = 1, AutoComplete = false });
                 
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                await queueClient.UnregisterMessageHandlerAsync(new TimeSpan(10));
            }
            SBMessage.ServiceBusMessage = UUUUMessage;
            return SBMessage.ServiceBusMessage;

            //SubscriptionClient subscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureSBEndpoint"), "RedPandaServices", _config.GetConnectionString("AzureSBQueueName"));
            //try
            //{
            //    subscriptionClient.RegisterMessageHandler(
            //        async (message, token) =>
            //        {
            //            var messageJson = Encoding.UTF8.GetString(message.Body);
            //            string tryplss = messageJson.ToString();
            //            //var updateMessage = JsonConvert.DeserializeObject<QueueMessage>(messageJson);

            //            Console.WriteLine($"Received message with productId: {tryplss}");

            //            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            //        },
            //        new MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
            //        { MaxConcurrentCalls = 1, AutoComplete = false });
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception: " + e.Message);
            //}
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

        public async Task<QueueMessage> ReceiveMessageAsync(bool isAzure = true)
        {
            SBMessage.ServiceBusMessage = "Azure Service Bus has no message!";

            if (isAzure == true)
            {
                await ReceiveSBMessages();
            }

            return SBMessage;
        }



        //static async Task MessageHandler(ProcessMessageEventArgs args)
        //{
        //    string body = args.Message.Body.ToString();
        //    Console.WriteLine($"Received: {body}");

        //    await args.CompleteMessageAsync(args.Message);
        //}

        //static Task ErrorHandler(ProcessErrorEventArgs args)
        //{
        //    Console.WriteLine(args.Exception.ToString());
        //    return Task.CompletedTask;
        //}
        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Error: {arg.Exception}");
            return Task.CompletedTask;
        }

    }
}
