using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;

namespace Indigo.Server.Models
{
    public class IndigoContext : DbContext
    {
        public IndigoContext (DbContextOptions<IndigoContext> options)
            : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
    }
}