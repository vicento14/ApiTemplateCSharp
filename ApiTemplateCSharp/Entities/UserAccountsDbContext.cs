using Microsoft.EntityFrameworkCore;

namespace ApiTemplateCSharp.Entities
{
    public class UserAccountsDbContext : DbContext
    {
        public UserAccountsDbContext() : base() { }
        public UserAccountsDbContext(DbContextOptions<UserAccountsDbContext> options) : base(options) { }
        public DbSet<UserAccounts> UserAccounts { get; set; }
    }
}
