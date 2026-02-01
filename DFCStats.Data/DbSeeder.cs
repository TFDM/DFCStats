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

            // Seeds categories table with some sample data only if the table is empty
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Id = new Guid("9F6A2C42-5521-473F-8A86-64192124F4E2"), Description = "Friendly", League = false, Cup = false, FootballLeague = false, NonLeague = false, PlayOff = false, OrderNo = 0 },
                    new Category { Id = new Guid("123BEE23-6359-415B-AC53-C62C49AD6EE7"), Description = "Football League", League = true, Cup = false, FootballLeague = true, NonLeague = false, PlayOff = false, OrderNo = 1 },
                    new Category { Id = new Guid("5B456693-82A4-409F-B443-CA0F976273A5"), Description = "Football League Play-Off", League = false, Cup = false, FootballLeague = true, NonLeague = false, PlayOff = true, OrderNo = 2 },
                    new Category { Id = new Guid("9082A2DF-AF2D-41AB-9AC3-796ED65F5483"), Description = "Non League", League = true, Cup = false, FootballLeague = false, NonLeague = true, PlayOff = false, OrderNo = 3 },
                    new Category { Id = new Guid("A0C64557-39BD-4418-8F96-83068AA3DCED"), Description = "Non League Play-Off", League = false, Cup = false, FootballLeague = false, NonLeague = true, PlayOff = true, OrderNo = 4 },
                    new Category { Id = new Guid("F242E648-07C0-46EF-8627-58A15D312850"), Description = "Cup", League = false, Cup = true, FootballLeague = false, NonLeague = false, PlayOff = false, OrderNo = 5 }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }

            // Seeds venues table with some sample data only if the table is empty
            if (!await dbContext.Venues.AnyAsync())
            {
                var venues = new List<Venue>
                {
                    new Venue { Id = new Guid("a1b2c3d4-e5f6-4789-9012-34567890abcd"), Description = "Home", ShortDescription = "H", OrderNo = 1 },
                    new Venue { Id = new Guid("b2c3d4e5-f678-4890-1234-567890abcdef"), Description = "Away", ShortDescription = "A", OrderNo = 2 },
                    new Venue { Id = new Guid("c3d4e5f6-7890-4901-2345-67890abcdef1"), Description = "Neutral", ShortDescription = "N", OrderNo = 3 }
                };

                await dbContext.Venues.AddRangeAsync(venues);
                await dbContext.SaveChangesAsync();
            }

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