using Microsoft.EntityFrameworkCore;

namespace MoviesCRUDApp.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        DbSet<Genre> Genres { get; set; }
        DbSet<Movie> Movies { get; set; }
    }
}
