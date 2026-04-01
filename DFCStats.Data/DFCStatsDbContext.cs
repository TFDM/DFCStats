using Microsoft.EntityFrameworkCore;
using DFCStats.Data.Entities;

namespace DFCStats.Data
{
    public class DFCStatsDBContext : DbContext
    {
        public DFCStatsDBContext(DbContextOptions<DFCStatsDBContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Participation> Participants { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonSeason> PeopleSeasons { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public DbSet<View_People> View_People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonSeason>().HasKey(x => new { x.PersonId, x.SeasonId });

            modelBuilder.Entity<Participation>()
                .HasOne(x => x.Person)
                .WithMany(x => x.Participation)
                .HasForeignKey(x => x.PersonId);

            modelBuilder.Entity<Participation>()
                .HasOne(x => x.ReplacedByPerson)
                .WithMany()
                .HasForeignKey(x => x.ReplacedByPersonId);

            // Configures SQL views so the entity maps to the actual sql view
            modelBuilder.Entity<View_People>().ToView("View_People");
        }
    }
}