namespace QueueReceiverService.Models
{
    public class QueueMessage
    {
        public string ServiceBusMessage { get; set; } = "Service Bus Queue is Empty";
        public string SQSMessage { get; set; } = "SQS Queue is Empty";
        public string? Message { get; set; }
    }
}
