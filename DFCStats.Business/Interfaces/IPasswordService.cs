namespace DFCStats.Business.Interfaces
{
    public interface IPasswordService
    {
        /// <summary>
        /// Checks a password matches basic complexity rules
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckPasswordComplexity(string password);

        /// <summary>
        /// Hashes a password using a salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password, string salt);

        /// <summary>
        /// Generates a random salt which can be used for password hashing
        /// </summary>
        /// <returns></returns>
        string GenerateRandomSalt();

        /// <summary>
        /// Validates a password against a password known to be valid - requires the salt
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <param name="hashedSaltedPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        bool ValidatePassword(string plainTextPassword, string hashedSaltedPassword, string salt);
    }
}