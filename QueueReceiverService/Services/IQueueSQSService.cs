namespace QueueReceiverService.Services
{
    public interface IQueueSQSService
    {
        public Task ReceiveSQSMessages(); 
    }
}
