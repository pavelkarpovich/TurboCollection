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
        public DbSet<PrivateTurboItem> PrivateTurboItems { get; set; }
        public DbSet<TurboItemStatus> TurboItemStatuses { get; set; }
        public DbSet<ExtraTurboItem> ExtraTurboItems { get; set; }
        public DbSet<ChatPost> ChatPosts { get; set; }
        public DbSet<Unread> Unreads { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
