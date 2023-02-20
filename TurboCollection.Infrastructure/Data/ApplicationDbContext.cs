using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboCollection.ApplicationCore.Entities;

namespace TurboCollection.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TurboItem> TurboItems { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
