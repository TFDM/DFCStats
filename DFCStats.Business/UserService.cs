using DFCStats.Data;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.DTOs;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Users;
using DFCStats.Business.MappingExtensions;

namespace DFCStats.Business
{
    public class UserService : IUserService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        
        public UserService(DFCStatsDBContext dFCStatsDBContext)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var query = _dfcStatsDbContext.Users.AsNoTracking().AsQueryable();

            query = query.Include(u => u.UserRoles)
                .ThenInclude(r => r.Role);

            return await query.Select(u => u.MapToUserDTO()!).ToListAsync();
        }
    }
}