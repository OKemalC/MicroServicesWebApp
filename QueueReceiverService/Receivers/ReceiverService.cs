using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using QueueReceiverService.Models;
using QueueReceiverService.Services;
using System.Text;

namespace QueueReceiverService.Receivers
{
    public class ReceiverService : IQueueReceiverService
    {
        private readonly IConfiguration _config;
        static IQueueClient queueClient;
        QueueMessage QueueMessage = new QueueMessage(); 
        static BasicAWSCredentials? credentials;
        static AmazonSQSClient? sqsClient;
        static ReceiveMessageRequest? request; 
        QueueMessage ReceivedMessage = new QueueMessage();

        public ReceiverService(IConfiguration config)
        {
            _config = config;

            queueClient = new QueueClient(_config.GetConnectionString("AzureSBEndpoint"), _config.GetConnectionString("AzureSBQueueName"));
            var messageandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            }; 

            credentials = new BasicAWSCredentials(_config.GetConnectionString("AwsAccessKey"), _config.GetConnectionString("AwsSecretKey"));
            sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest2);
            request = new ReceiveMessageRequest()
            {
                QueueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue",
                WaitTimeSeconds = 10,
            };
        }
        public async Task<QueueMessage> ReceiveMessageAsync(bool sbAzure = true, bool sqsAmazon = true)
        {
            if (sbAzure==true && sqsAmazon==true)
            {
                await ReceiveMessageSBAsync();
                await ReceiveMessageSQSAsync();
            }
            return QueueMessage;


        }
        public async Task<QueueMessage>  ReceiveMessageSBAsync()
        {
            try
            {
                queueClient.RegisterMessageHandler(
                     async (message, token) =>
                     {
                         var messageJson = Encoding.UTF8.GetString(message.Body);
                         string tryplss = messageJson.ToString();
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
                return null;
            }
            finally
            {
                await queueClient.UnregisterMessageHandlerAsync(new TimeSpan(10)); 
            } 
            return QueueMessage;
        }

        public async Task<QueueMessage> ReceiveMessageSQSAsync(bool isAmazon = true)
        {
            QueueMessage sqsMessage = new QueueMessage();
            var response = await sqsClient.ReceiveMessageAsync(request);
            try
            {
                if (response.Messages.Count > 0)
                {
                    string sqsmessage = response.Messages[0].Body.ToString();
                    ReceivedMessage.SQSMessage = response.Messages[0].Body.ToString();
                    //SQSMessage.SQSMessage = sqsmessage;
                    Console.WriteLine($"Amazon {sqsmessage}");
                    sqsMessage.SQSMessage = response.Messages.ToString();
                    await sqsClient.DeleteMessageAsync(_config.GetConnectionString("AmazonQueueUrl"), response.Messages[0].ReceiptHandle);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MyGlobalVariables.MyGlobalString = ReceivedMessage.SQSMessage.ToString();

            return QueueMessage;
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Error: {arg.Exception}");
            return Task.CompletedTask;
        } 
    }

}
