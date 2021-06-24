using BC = BCrypt.Net.BCrypt;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace oauth_service.Helpers
{
    public class Crypto : ICrypto
    {
        protected string HashPassword;

        public Crypto(string hashPassword)
        {
            HashPassword = hashPassword ?? throw new ArgumentNullException(nameof(HashPassword));
        }

        public bool VerifyPassword(string password)
        {
            return BC.Verify(password, EncryptPassword(password));
        }

        public string EncryptPassword(string password)
        {
            return BC.HashPassword(password);
        }
    }

    public interface ICrypto
    {
        bool VerifyPassword(string password);
        string EncryptPassword(string password);
    }
}
