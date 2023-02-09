using System.ComponentModel.DataAnnotations;

namespace Assessment.Models.Requests
{
    public class ContactRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
