using DFCStats.Data;
using DFCStats.Data.Repositories;
using DFCStats.Data.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly DFCStatsDBContext _dbContext;

    // Repositories set via dependancy injection
    public IClubRepository ClubRepository { get; }

    public UnitOfWork(
        DFCStatsDBContext dbcontext, 
        IClubRepository clubRepository)
    {
        _dbContext = dbcontext;
        ClubRepository = clubRepository;
    }

    /// <summary>
    /// Commit changes to the database
    /// </summary>
    /// <returns></returns>
    public async Task CommitChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}