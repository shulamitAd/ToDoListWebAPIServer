using Microsoft.EntityFrameworkCore;

namespace WebAPIServer.Models
{
    public class TODODBContext:DbContext
    {
        public TODODBContext(DbContextOptions options) : base(options) { }


        public DbSet<TODOItem> TODOItems { get; set; }
    }
}
