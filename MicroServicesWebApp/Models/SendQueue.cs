using System.ComponentModel.DataAnnotations;

namespace MicroServicesWebApp.Models
{
    public class SendQueue
    {
        [Required]
        public string? Messages { get; set; }
    }
}
