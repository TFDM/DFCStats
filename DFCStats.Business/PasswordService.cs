using DFCStats.Business.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace DFCStats.Business
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Checks a password matches basic complexity rules
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckPasswordComplexity(string password)
        {
            // Check if the password is over 8 characters long
            if (password.Length < 8)
                return false;

            // Check if the string contains at least one uppercase character
            if (!password.Any(char.IsUpper))
                return false;

            // Check if the string contains at least one number
            if (!password.Any(char.IsDigit))
                return false;

            // If all checks pass, return true
            return true;
        }

        /// <summary>
        /// Hashes a password using a salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string HashPassword(string password, string salt)
        {
            byte[] hashedPassword = Rfc2898DeriveBytes.Pbkdf2(
                password,
                Encoding.UTF8.GetBytes(salt),
                iterations: 210_000, // OWASP 2023 recommendation for SHA256
                HashAlgorithmName.SHA256,
                32); // 256-bit output

            return Convert.ToHexString(hashedPassword);

            // // Concatenate the password and the salt
            // var saltedPassword = string.Concat(password, salt);

            // // Returns the salted password as bytes
            // var saltedPasswordAsAsBytes = Encoding.UTF8.GetBytes(saltedPassword);

            // // Hash the password as bytes
            // SHA256 hash = SHA256.Create();
            // byte[] hashedPassword = hash.ComputeHash(saltedPasswordAsAsBytes);

            // // Convert the bytes back to a string
            // return Convert.ToHexString(hashedPassword);
        }

        /// <summary>
        /// Generates a random salt which can be used for password hashing
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomSalt()
        {
            // Allocate a 16-byte buffer (128 bits of security entropy)
            // Fetch truly unpredictable random bytes directly from the Operating System
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);

            // Convert the secure bytes into a clean, alphanumeric text string
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Validates a password against a password known to be valid - requires the salt
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <param name="hashedSaltedPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public bool ValidatePassword(string plainTextPassword, string hashedSaltedPassword, string salt)
        {
            // Produce a hash of the plain text password and salt
            string computedHash = HashPassword(plainTextPassword, salt);

            // Convert both hex hashes into byte arrays for a secure comparison
            byte[] computedHashInBytes = Convert.FromHexString(computedHash);
            byte[] hashedSaltedPasswordInBytes = Convert.FromHexString(hashedSaltedPassword);

            // Use FixedTimeEquals to securely compare the arrays.
            // This takes the exact same amount of time to compute regardless of whether 
            // the password matches or fails, completely neutralizing timing attacks.
            return CryptographicOperations.FixedTimeEquals(computedHashInBytes, hashedSaltedPasswordInBytes);
        }
    }
}