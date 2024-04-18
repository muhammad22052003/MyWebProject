using Microsoft.AspNetCore.Identity;
using WebProject.interfaces.auth;
using SHA3.Net;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace WebProject.Provaiders
{
    public class CustomPasswordHasher : ICustomPasswordHasher
    {
        public string Generate(string password)
        {
            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            string hashedPassword = string.Empty;

            using (Sha3 sha3 = Sha3.Sha3256())
            {
                hashedPassword = BitConverter.ToString(sha3.ComputeHash(passwordByte))
                                             .Replace("-", string.Empty);
            }

            return hashedPassword;
        }

        public bool Verify(string password, string hashedPassword)
        {
            string result = Generate(password);

            if (hashedPassword == result) { return true; }

            return false;
        }
    }
}
