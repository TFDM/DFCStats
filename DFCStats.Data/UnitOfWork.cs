using DFCStats.Data;
using DFCStats.Data.Repositories;
using DFCStats.Data.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly DFCStatsDBContext _dbContext;

    public IClubRepository ClubRepository { get; private set; }

    public UnitOfWork(DFCStatsDBContext dbcontext)
    {
        _dbContext = dbcontext;
        ClubRepository = new ClubRepository(_dbContext);
    }

    public async Task CommitChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}