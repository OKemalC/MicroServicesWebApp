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
        QueueMessage SQSMessage = new QueueMessage();
        

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
            try
            {
                if (response.Messages.Count > 0)
                {
                    string sqsmessage = response.Messages[0].Body.ToString();
                    //SQSMessage.SQSMessage = sqsmessage;
                    Console.WriteLine($"Amazon {sqsmessage}");
                    sqsMessage.Message = response.Messages.ToString();
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
            }
            //return SQSMessage.SQSMessage;
            // await sqsClient.DeleteMessageAsync(_config.GetConnectionString("AwsAccessKey"), response.Messages[0].ReceiptHandle); 
        }
        //public async Task<QueueMessage> ReceiveMessageAsync(bool isAzure = true)
        //{
        //    SQSMessage.SQSMessage = "amazon sage!";

        //    if (isAzure == true)
        //    {
        //        await ReceiveSQSMessages();
        //    }

        //    return SQSMessage;
        //}
    }
}

