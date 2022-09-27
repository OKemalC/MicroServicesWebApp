namespace QueueReceiverService.Models
{
    public class QueueMessage
    {
        public string? ServiceBusMessage { get; set; }
        public string? SQSMessage { get; set; }
        public string? Message { get; set; }
    }
}
