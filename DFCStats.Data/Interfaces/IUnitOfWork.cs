namespace DFCStats.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IClubRepository ClubRepository { get; }
        Task CommitChanges();
    }
}

