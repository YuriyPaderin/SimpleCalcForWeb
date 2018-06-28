using Microsoft.EntityFrameworkCore;

namespace SimpleCalcForWeb.Models
{ 
    public class Repository : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public Repository(DbContextOptions<Repository> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
