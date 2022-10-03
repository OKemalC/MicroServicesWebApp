using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using QueueReceiverService.Models;
using QueueReceiverService.Services;
using System.Diagnostics;

namespace QueueReceiverService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;  
        private readonly IQueueReceiverService _queueService;
        QueueMessage model = new QueueMessage();

        public HomeController(ILogger<HomeController> logger, IQueueReceiverService queueService)
        {
            _logger = logger; 
            _queueService= queueService;
        }

        public async Task<IActionResult> Index()
        {
            model = await _queueService.ReceiveMessageAsync();
            ViewBag.sqs = model.SQSMessage;
            return View(model); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        } 
        public string ModelReturn()
        { 
            ViewBag.sqs = model.SQSMessage;
            return  model.SQSMessage;
        }
 
    }
}