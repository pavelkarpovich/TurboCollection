using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboCollection.ApplicationCore.Entities;

namespace TurboCollection.Infrastructure.Data
{
    public sealed class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext applicationDbContext, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (!await applicationDbContext.Collections.AnyAsync())
                {
                    await applicationDbContext.AddRangeAsync(GetPreconfiguredCollections());
                    await applicationDbContext.SaveChangesAsync();
                }

                if (!await applicationDbContext.TurboItems.AnyAsync())
                {
                    await applicationDbContext.AddRangeAsync(GetPreconfiguredTurboItems());
                    await applicationDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability > 10) throw;
                retryForAvailability++;
                await SeedAsync(applicationDbContext, retryForAvailability);
            }
        }

        private static IEnumerable<TurboItem> GetPreconfiguredTurboItems()
        {
            return new List<TurboItem>()
            {
                new(1, 1, "001.jpg", "Opel", "Ascona CD", speed: 187, engineCapacity: 1796, horsePower: 115)
            };
        }

        private static IEnumerable<Collection> GetPreconfiguredCollections()
        {
            return new List<Collection>()
            {
                new("1-50"),
                new("51-120"),
                new("121-180"),
                new("181-260"),
                new("261-330"),
                new("Super 331-400"),
                new("Super 401-470"),
                new("Super 471-540"),
                new("Sport 1-70"),
                new("Sport 71-140"),
                new("Sport 141-210"),
                new("Classic 1-70"),
                new("Classic 71-140"),
            };
        }
    }
}
