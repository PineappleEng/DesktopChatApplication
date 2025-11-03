using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Security
    {
        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(bytes);
            return bytes;
        }

        public static string HashPassword(string password)
        {
            byte[] salt = GenerateRandomBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static bool ComparePassword(string entered, string stored)
        {
            try
            {
                var parts = stored.Split(':');
                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHash = Convert.FromBase64String(parts[1]);

                var pbkdf2 = new Rfc2898DeriveBytes(entered, salt, 100_000, HashAlgorithmName.SHA256);
                byte[] computedHash = pbkdf2.GetBytes(32);

                return storedHash.SequenceEqual(computedHash);
            }
            catch
            {
                return false;
            }
        }

        public static bool VerifyPassword(string entered, string stored)
        {
            return ComparePassword(HashPassword(entered), stored);
        }
    }
}
