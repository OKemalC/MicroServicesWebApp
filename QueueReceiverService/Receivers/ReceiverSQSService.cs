using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using QueueReceiverService.Models;
using System.Text;

namespace QueueReceiverService.Services
{
    public class ReceiverSQSService : IQueueSQSService
    {
        private readonly IConfiguration _config; 
        static BasicAWSCredentials? credentials;
        static AmazonSQSClient? sqsClient;
        static ReceiveMessageRequest? request; 
        

        public ReceiverSQSService(IConfiguration config)
        {
            _config = config; 

            credentials = new BasicAWSCredentials(_config.GetConnectionString("AwsAccessKey"), _config.GetConnectionString("AwsSecretKey"));
            sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest2);
            request = new ReceiveMessageRequest()
            {
                QueueUrl = "https://sqs.eu-west-2.amazonaws.com/811978060585/myqueue",
                WaitTimeSeconds = 10,
            };
 
        }
        public async Task ReceiveSQSMessages()
        {
            QueueMessage sqsMessage = new QueueMessage();
            var response = await sqsClient.ReceiveMessageAsync(request);
            if (response.Messages.Count > 0) sqsMessage.Message = response.Messages.ToString();

        } 
    }
}
