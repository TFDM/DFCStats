using DFCStats.Domain.DTOs.Users;

namespace DFCStats.Business.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}