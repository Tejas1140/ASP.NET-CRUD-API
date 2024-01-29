using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ContactEntity : DbContext
    {
        public ContactEntity(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ContactDetails> Contact { get; set; }
    }
}