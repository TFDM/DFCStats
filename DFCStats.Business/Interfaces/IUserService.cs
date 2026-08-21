using DFCStats.Domain.DTOs.Users;

namespace DFCStats.Business.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<UserDTO> RegisterUserAsync(UserDTO userDTO);

        /// <summary>
        /// Gets a user record by e-mail address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<UserDTO?> GetUserByEmailAddressAsync(string emailAddress);

        Task<List<UserDTO>> GetAllUsersAsync();
    }
}