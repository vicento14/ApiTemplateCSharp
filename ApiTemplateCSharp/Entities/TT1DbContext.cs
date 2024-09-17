using Microsoft.EntityFrameworkCore;

namespace ApiTemplateCSharp.Entities
{
    public class TT1DbContext : DbContext
    {
        public TT1DbContext() : base() { }
        public TT1DbContext(DbContextOptions<TT1DbContext> options) : base(options) { }
        public DbSet<TT1> TT1 { get; set; }
    }
}
