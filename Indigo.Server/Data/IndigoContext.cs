using Indigo.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Indigo.Server.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Database context to hold database model
    /// </summary>
    public class IndigoContext : DbContext
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates DbContext with options
        /// </summary>
        /// <param name="options"></param>
        public IndigoContext (DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or Sets the set of all pages within the database
        /// </summary>
        public DbSet<Page> Pages { get; set; }
    }
}