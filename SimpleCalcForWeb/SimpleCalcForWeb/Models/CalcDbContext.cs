using Microsoft.EntityFrameworkCore;

namespace SimpleCalcForWeb.Models
{ 
    public class CalcDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public CalcDbContext(DbContextOptions<CalcDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
