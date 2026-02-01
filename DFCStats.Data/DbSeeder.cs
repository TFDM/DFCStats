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
                    new Club { Id = new Guid("c87dada3-36b9-4d4d-b7ae-57c6820a200d"), Name = "FC United of Manchester" },
                    new Club { Id = new Guid("c299f45c-2faf-4bf0-bfb8-110774261258"), Name = "Spennymoor Town" },
                    new Club { Id = new Guid("366fdf1f-5eb6-449b-97ff-412658e718d3"), Name = "Hartlepool United" }
                };

                await dbContext.Clubs.AddRangeAsync(clubs);
                await dbContext.SaveChangesAsync();
            }

            // Seeds seasons table with some sample data only if the table is empty
            if (!await dbContext.Seasons.AnyAsync())
            {
                var seasons = new List<Season>
                {
                    new Season { Id = new Guid("1f7b61c7-d523-4888-836f-87f79ab64f95"), Description = "2022/23" },
                    new Season { Id = new Guid("ddb2a4f0-5888-4d1c-b9f5-ff3858adba29"), Description = "2023/24" }
                };

                await dbContext.Seasons.AddRangeAsync(seasons);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}