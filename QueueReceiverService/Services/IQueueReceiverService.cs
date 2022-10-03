using QueueReceiverService.Models;

namespace QueueReceiverService.Services
{
    public interface IQueueReceiverService
    {
        //public Task ReceiveSBMessages();
        //Task<QueueMessage> ReceiveMessageSBAsync(bool isAzure = true);

        //public Task ReceiveSQSMessages(); 
        //Task<QueueMessage> ReceiveMessageSQSAsync(bool isAmazon = true);
        Task<QueueMessage> ReceiveMessageAsync(bool sbAzure = true,bool sqsAmazon=true);
       
    }
}
