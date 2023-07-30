using Microsoft.EntityFrameworkCore;
using Notes.API.Model.Entity;

namespace Notes.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
