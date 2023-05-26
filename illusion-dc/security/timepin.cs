using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace illusion_dc.security
{
    internal class timepin
    {

        //this shit is not secure :fire: :skull:
        public static string GetPin()
        {
            DateTime currentTimeUtc = DateTime.UtcNow;
            return sha256(currentTimeUtc.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)).ToLower();
        }

        private static string sha256(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = SHA256.Create().ComputeHash(inputBytes);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            return hashString;
        }
    }
}
