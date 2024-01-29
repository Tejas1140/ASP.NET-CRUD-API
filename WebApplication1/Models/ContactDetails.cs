using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ContactDetails
    {
        [Key]
        public long Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public long Phone { get; set; }           
        public string Address { get; set; }

    }
}
