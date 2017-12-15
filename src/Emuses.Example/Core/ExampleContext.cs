using Emuses.Example.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Emuses.Example.Core
{
    public class ExampleContext : DbContext
    {
        public virtual DbSet<EmusesSession> EmusesSessions { get; set; }

        public ExampleContext()
        {
        }

        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
        {
        }
    }
}