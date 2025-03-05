using Microsoft.EntityFrameworkCore;
using ODataWebAPI.Models;

namespace ODataWebAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
