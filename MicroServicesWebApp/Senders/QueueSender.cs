using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using MicroServicesWebApp.Models;
using Microsoft.Azure.ServiceBus;
using QueueSenderService.Services;
using System.Text;
using System.Text.Json;

namespace QueueSenderService.Senders
{
    public class QueueSender : IQueueSenderService
    {
        private readonly IConfiguration _config;

        public QueueSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendMessageSBAsync(SendQueue message)
        {
            var queueClient = new QueueClient(_config.GetConnectionString("AzureSBEndpoint"),_config.GetConnectionString("AzureSBQueueName"));
            string messageBody = JsonSerializer.Serialize(message);
            var messages = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(messages);
        }

        public async Task SendMessageSQSAsync(SendQueue message)
        { 
            var credentials = new BasicAWSCredentials(_config.GetConnectionString("AwsAccessKey"), _config.GetConnectionString("AwsSecretKey"));
            var client = new AmazonSQSClient(credentials, RegionEndpoint.EUWest2);

            var request = new SendMessageRequest()
            {
                QueueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue",
                MessageBody = JsonSerializer.Serialize(message)
            };
            await client.SendMessageAsync(request);
        }
      
    }
}
