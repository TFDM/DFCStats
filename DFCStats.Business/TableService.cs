using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Business.Interfaces;

namespace DFCStats.Business
{
    public class TableService : ITableService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        
        public TableService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }
    }
}