using MicroServicesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using QueueSenderService.Services;

namespace MicroServicesWebApp.Controllers
{
    public class QueueController : Controller
    {
        private readonly ILogger<QueueController> _logger;
        private readonly IQueueSenderService _queueSender;

        public QueueController(ILogger<QueueController> logger,IQueueSenderService queueSender)
        {
            _logger = logger;
            _queueSender = queueSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync(SendQueue message)
        {
            await _queueSender.SendMessageSQSAsync(message);
            await _queueSender.SendMessageSBAsync(message); 
            return RedirectToAction("Index"); 
        }
    }
}
