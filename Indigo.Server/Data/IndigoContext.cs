using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;

namespace Indigo.Server.Models
{
    /// <summary>
    /// Database context to hold database model
    /// </summary>
    public class IndigoContext : DbContext
    {
        /// <summary>
        /// Creates DbContext with options
        /// </summary>
        /// <param name="options"></param>
        public IndigoContext (DbContextOptions<IndigoContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or Sets the set of all pages within the database
        /// </summary>
        public DbSet<Page> Pages { get; set; }
    }
}