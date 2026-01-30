using Microsoft.EntityFrameworkCore;
using DFCStats.Data.Entities;

namespace DFCStats.Data
{
    public class DFCStatsDBContext : DbContext
    {
        public DFCStatsDBContext(DbContextOptions<DFCStatsDBContext> options) : base(options) { }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Season> Seasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}