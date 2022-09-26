using MicroServicesWebApp.Models;

namespace QueueSenderService.Services
{
    public interface IQueueSenderService
    {  
        Task SendMessageSQSAsync(SendQueue message);
        Task SendMessageSBAsync(SendQueue message);
        
    }
}
