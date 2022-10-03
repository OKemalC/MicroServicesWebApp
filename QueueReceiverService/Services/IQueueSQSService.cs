using QueueReceiverService.Models;

namespace QueueReceiverService.Services
{
    public interface IQueueSQSService
    {
        //public Task ReceiveSQSMessages(); 
        Task<QueueMessage> ReceiveMessageAsync(bool isAzure = true);
    }
}
