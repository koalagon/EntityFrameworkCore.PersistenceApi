using Microsoft.EntityFrameworkCore.PersistenceApi;
#pragma warning disable CS8618

namespace Microsoft.EntityFrameworkCore.WebApplication
{
    public class ApplicationDbContext : DbContext, IEpaDbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        DbSet<Order> Orders { get; set; }
    }
}
