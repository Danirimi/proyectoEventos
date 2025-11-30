using System;
using System.Security.Cryptography;

namespace proyectoEventos.Modelo
{
    public static class PasswordSecurity
    {
        public static string HashPassword(string password)
        {
            // Generar salt aleatorio
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Combinar password + salt y hacer hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combinar salt + hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Extraer bytes
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Extraer salt
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                // Hash la password ingresada con el mismo salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(20);

                // Comparar hashes
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}