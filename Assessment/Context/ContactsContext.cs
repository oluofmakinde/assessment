using Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Context
{
    public class ContactsContext : DbContext
    {

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasIndex(x => x.Name).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Contacts");
        }
    }
}
