using DFCStats.Domain.DTOs.Users;

namespace DFCStats.Business.Interfaces
{
    [Flags]
    public enum UserIncludes
    {
        // If new flags are required double the previous number
        None = 0,
        Roles = 1,
        All = Roles
    }

    public interface IUserService
    {
        /// <summary>
        /// Get a user by their id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<UserDTO?> GetUserById(Guid id, UserIncludes includes = UserIncludes.None);

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<UserDTO> RegisterUserAsync(UserDTO userDTO);

        /// <summary>
        /// Updates a user without changing their password
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<UserDTO> UpdateUserAsync(UserDTO userDTO);

        /// <summary>
        /// Gets a user record by e-mail address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<UserDTO?> GetUserByEmailAddressAsync(string emailAddress);
    }
}