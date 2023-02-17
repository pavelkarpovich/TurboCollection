using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TurboCollection.Infrastructure.Data;

namespace TurboCollection.Infrastructure
{
    public static class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<AccountDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("AccountDbConnection")));
            services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection")));
        }
    }
}
