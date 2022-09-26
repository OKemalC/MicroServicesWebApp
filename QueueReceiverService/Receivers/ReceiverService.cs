using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Azure.ServiceBus;
using QueueReceiverService.Models;
using System.Text;

namespace QueueReceiverService.Services
{
    public class ReceiverService : IQueueReceiverService
    {
        private readonly IConfiguration _config;
        static IQueueClient? queueClient;
        static BasicAWSCredentials? credentials;
        static AmazonSQSClient? client;
        static ReceiveMessageRequest? request;
        static MessageHandlerOptions? messageandlerOptions;

        public ReceiverService(IConfiguration config)
        {
            _config = config;

            queueClient = new QueueClient(_config.GetConnectionString("AzureSBEndpoint"), _config.GetConnectionString("AzureSBQueueName"));

            credentials = new BasicAWSCredentials(_config.GetConnectionString("AwsAccessKey"), _config.GetConnectionString("AwsSecretKey"));
            client = new AmazonSQSClient(credentials, RegionEndpoint.EUWest2);
            request = new ReceiveMessageRequest()
            {
                QueueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue",
                WaitTimeSeconds = 10,
            };
            messageandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

        }
        public async Task ReceiveSQSMessages()
        {
            QueueMessage sqsMessage = new QueueMessage();
            var response = await client.ReceiveMessageAsync(request);
            if (response.Messages.Count > 0) sqsMessage.Message = response.Messages.ToString();

        }

        public async Task ReceiveSBMessages()
        {
            queueClient.RegisterMessageHandler(ProcessMessageAsync, messageandlerOptions);


            await queueClient.CloseAsync();
        }
        private static async Task ProcessMessageAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            //PersonModel person = new PersonModel();
            var jsonString = Encoding.UTF8.GetString(message.Body);
            // PersonModel person = JsonSerializer.Deserialize<PersonModel>(jsonString);  
            string peros = jsonString.ToString();
            //person.FirstName = peros;
            //Console.WriteLine($"Recieved: {person.FirstName}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Error: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}
