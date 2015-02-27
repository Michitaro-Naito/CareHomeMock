using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Helper
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Returns an alphanumeric password like "Abc123DeFG".
        /// Does not contain 1, l, I, 0, o and O.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GeneratePassword(int length)
        {
            var chars = new List<char>();
            for (var n = 0; n < 10; n++)
            {
                chars.Add((char)('0' + n));
            }
            for (var n = 0; n < 26; n++)
            {
                chars.Add((char)('a' + n));
                chars.Add((char)('A' + n));
            }
            chars.Remove('1');
            chars.Remove('l');
            chars.Remove('I');
            chars.Remove('0');
            chars.Remove('o');
            chars.Remove('O');

            var password = "";
            var rand = new Random();
            for (var n = 0; n < length; n++)
            {
                var index = rand.Next(chars.Count);
                password += chars[index];
            }
            return password;
        }
    }
}