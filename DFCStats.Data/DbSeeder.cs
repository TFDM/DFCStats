using Microsoft.Extensions.DependencyInjection;
using DFCStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Creates a scope to resolve the DbContext
            using var scope = serviceProvider.CreateAsyncScope();
            var dBContext = scope.ServiceProvider.GetRequiredService<DFCStatsDBContext>();

            // Seeds clubs table with some sample data only if the table is empty
            if (!await dBContext.Clubs.AnyAsync())
            {
                dBContext.Clubs.AddRange(
                    new Club { Name = "Hartlepool United" },
                    new Club { Name = "Spennymoor Town" },
                    new Club { Name = "AFC Telford"}
                );

                await dBContext.SaveChangesAsync();
            }
        }
    }
}