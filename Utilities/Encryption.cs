using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace authentication_service.Utilities
{
    public class Encryption
    {
        [ComVisibleAttribute(true)]
        public class SHA256Managed : SHA256
        {
            public override void Initialize()
            {
                throw new NotImplementedException();
            }

            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                throw new NotImplementedException();
            }

            protected override byte[] HashFinal()
            {
                throw new NotImplementedException();
            }
        }

        public static string CreateSalt(int size)
        {
            var rng = RandomNumberGenerator.Create();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256 sha256hashstring = SHA256.Create();
            byte[] hash = sha256hashstring.ComputeHash(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
