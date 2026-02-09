using Microsoft.EntityFrameworkCore;
using DFCStats.Data.Entities;

namespace DFCStats.Data
{
    public class DFCStatsDBContext : DbContext
    {
        public DFCStatsDBContext(DbContextOptions<DFCStatsDBContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonSeason> PeopleSeasons { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonSeason>().HasKey(x => new { x.PersonId, x.SeasonId });
        }
    }
}