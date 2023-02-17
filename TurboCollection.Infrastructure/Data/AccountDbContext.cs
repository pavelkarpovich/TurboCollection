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
    public class AccountDbContext : IdentityDbContext<Account>
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
        }
    }
}
