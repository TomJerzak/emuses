using Microsoft.EntityFrameworkCore;

namespace Emuses.Example.Core
{
    public class ExampleContext : DbContext
    {
        public virtual DbSet<Session> Sessions { get; set; }

        public ExampleContext()
        {
        }

        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
        {
        }
    }
}