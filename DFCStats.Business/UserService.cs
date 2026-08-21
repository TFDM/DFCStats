using DFCStats.Data;
using DFCStats.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using DFCStats.Domain.DTOs;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Users;
using DFCStats.Business.MappingExtensions;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.Roles;

namespace DFCStats.Business
{
    public class UserService : IUserService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        private readonly IPasswordService _passwordService;
        private readonly IRoleService _roleService;
        
        public UserService(DFCStatsDBContext dFCStatsDBContext, IPasswordService passwordService, IRoleService roleService)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _passwordService = passwordService;
            _roleService = roleService;
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        public async Task<UserDTO> RegisterUserAsync(UserDTO userDTO)
        {
            // Check the password and confirm password match
            if (userDTO.Password != userDTO.ConfirmPassword)
                throw new DFCStatsException("Passwords must match");

            // Look for a user with the same email address so we can check its not in use already
            var user = await GetUserByEmailAddressAsync(userDTO.EmailAddress);

            // Check if the user is null or not
            if (user != null)
                // If the user is not null then the email address has already been used
                throw new DFCStatsException($"{userDTO.EmailAddress} can't be used");

            // Check the password passes basic complexitty rules
            if (!_passwordService.CheckPasswordComplexity(userDTO.Password))
                // Passowrd failed basic complexity checks and can't be used
                throw new DFCStatsException($"Password must be longer than {_passwordService.MinPasswordLength} characters, have at least one uppercase character and one number");

            // Validate the selected roles to ensure they exist in the database
            await ValidateRoles(userDTO.Roles);

            // Generate a random salt for the account
            var salt = _passwordService.GenerateRandomSalt();

            // Generate hashed password for storing in the database
            var hashedPwd = _passwordService.HashPassword(userDTO.Password, salt);

            // Create user-role assignments only after every submitted role has been validated.
            var userRoles = await CreateUserRoleAssignmentsAsync(userDTO.Roles?.Select(ur => ur.Id).ToList());

            // Create the user with the dto and add the user roles
            var newUser = new User()
            {
                EmailAddress = userDTO.EmailAddress,
                Password = hashedPwd,
                Salt = salt,
                AllowLogin = userDTO.AllowLogin,
                UserRoles = userRoles
            };

            // Save the changes to the database
            await _dfcStatsDbContext.Users.AddAsync(newUser);
            await _dfcStatsDbContext.SaveChangesAsync();

            // Map the newly created user to a userDTO and return it
            return newUser.MapToUserDTO()!;
        }

        /// <summary>
        /// Gets a user record by e-mail address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<UserDTO?> GetUserByEmailAddressAsync(string emailAddress)
        {
            var query = _dfcStatsDbContext.Users.AsNoTracking().AsQueryable();

            // Run the query and map the entity to a DTO and return it
            var user = await query.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
            return user?.MapToUserDTO();
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var query = _dfcStatsDbContext.Users.AsNoTracking().AsQueryable();

            query = query.Include(u => u.UserRoles)
                .ThenInclude(r => r.Role);

            return await query.Select(u => u.MapToUserDTO()!).ToListAsync();
        }

        /// <summary>
        /// Validates the roles to ensure they are all present in the database
        /// </summary>
        /// <param name="roleDTOs"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task ValidateRoles(List<RoleDTO>? roleDTOs)
        {
            // If there are no roleDTOs there is nothing to validate
            if (roleDTOs == null)
                return;

            // Extract role Ids and remove duplicates before checking them.
            var roleIds = roleDTOs
                .Select(role => role.Id)
                .Distinct()
                .ToList();

            // Ask the role service which submitted IDs are not present in the database.
            var missingRoleIds = await _roleService.GetMissingRoleIdsAsync(roleIds);

            // Reject the registration instead of silently ignoring invalid role IDs.
            if (missingRoleIds.Count > 0)
            {
                throw new DFCStatsException("One or more selected roles do not exist.");
            }
        }

        /// <summary>
        /// Creates user-role assignments for the supplied role ids
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        private async Task<List<UserRole>> CreateUserRoleAssignmentsAsync(List<Guid>? roleIds)
        {
            // If the role ids are null return a new list of UserRole entities
            if (roleIds == null)
                return new List<DFCStats.Data.Entities.UserRole>();

            // Find all the roles with matching Ids
            var roles = await _dfcStatsDbContext.Roles
                .Where(role => roleIds.Contains(role.Id))
                .ToListAsync();

            // Create the user roles list and add each of the roles
            var userRoles = roles.Select(role => new UserRole
            {
                Id = Guid.NewGuid(),
                Role = role
            }).ToList();

            // Return the list of user role entities
            return userRoles;
        }
    }
}