using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Indigo.Core;

namespace Indigo.Server.Models
{
    public class IndigoContext : DbContext
    {
        public IndigoContext (DbContextOptions<IndigoContext> options)
            : base(options)
        {
        }

        public DbSet<Indigo.Core.Page> Pages { get; set; }
    }
}
