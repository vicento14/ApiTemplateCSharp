using Microsoft.EntityFrameworkCore;

namespace ApiTemplateCSharp.Entities
{
    public class TT2DbContext : DbContext
    {
        public TT2DbContext() : base() { }
        public TT2DbContext(DbContextOptions<TT2DbContext> options) : base(options) { }
        public DbSet<TT2> TT2 { get; set; }
    }
}
