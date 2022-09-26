namespace QueueReceiverService.Services
{
    public interface IQueueReceiverService
    {
        public Task ReceiveSQSMessages();
        public Task ReceiveSBMessages();
    }
}
