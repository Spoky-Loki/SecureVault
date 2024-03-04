using Microsoft.EntityFrameworkCore;

namespace SecureVault.Models
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CreditCard> CreditCards { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) {}
    }
}
