using Microsoft.EntityFrameworkCore;

namespace Contacts.Web.API.Models.ContextConfiguration
{
    public class ContactsWebAPIContext : DbContext
    {
        public ContactsWebAPIContext(DbContextOptions<ContactsWebAPIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting the email as 'unique' in the database
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique(true);
        }

        public DbSet<Skill> SkillItems { get; set; }
        public DbSet<Contact> ContactItems { get; set; }
    }
}
