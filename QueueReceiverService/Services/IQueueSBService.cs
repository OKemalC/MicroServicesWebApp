namespace QueueReceiverService.Services
{
    public interface IQueueSBService
    {
        public Task ReceiveSBMessages();
    }
}
