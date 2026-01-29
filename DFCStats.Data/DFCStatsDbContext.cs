using DFCStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DFCStats.Data
{
    public class DFCStatsDBContext : DbContext
    {
        public DFCStatsDBContext(DbContextOptions<DFCStatsDBContext> options) : base(options) { }

        public DbSet<DFCStats.Domain.Entities.Club> Clubs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}