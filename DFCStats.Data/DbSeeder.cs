using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DFCStats.Data.Entities;

namespace DFCStats.Data
{
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with some test data for development purposes
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Creates a scope to resolve the DbContext
            using var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DFCStatsDBContext>();

            // Seeds clubs table with some sample data only if the table is empty
            if (!await dbContext.Clubs.AnyAsync())
            {
                var clubs = new List<Club>
                {
                    new Club { Name = "FC United of Manchester" },
                    new Club { Name = "Spennymoor Town" },
                    new Club { Name = "Hartlepool United" }
                };

                await dbContext.Clubs.AddRangeAsync(clubs);
                await dbContext.SaveChangesAsync();
            }

            // Seeds seasons table with some sample data only if the table is empty
            if (!await dbContext.Seasons.AnyAsync())
            {
                var seasons = new List<Season>
                {
                    new Season { Description = "2022/23" },
                    new Season { Description = "2023/24" }
                };

                await dbContext.Seasons.AddRangeAsync(seasons);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}