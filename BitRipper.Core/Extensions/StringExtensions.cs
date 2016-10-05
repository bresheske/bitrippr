using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Core.Extensions
{
    public static class StringExtensions
    {
        private const string SALT = "&4P)2-Npna%^oye";
        public static string ToMD5(this string input)
        {
            return input.ToMD5(string.Empty);
        }
        public static string ToMD5(this string input, string additionalsalt)
        {
            var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(additionalsalt + input + SALT));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
    }
}
