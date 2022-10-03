using QueueReceiverService.Models;

namespace QueueReceiverService.Services
{
    public interface IQueueSBService
    {
        //public Task ReceiveSBMessages();
        Task<QueueMessage> ReceiveMessageAsync(bool isAzure = true);

    }
}
