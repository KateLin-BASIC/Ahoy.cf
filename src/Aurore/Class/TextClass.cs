using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Aurore.Class
{
    public class TextClass
    {
        public static string GenerateRandomString(int length)
        {
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string result = string.Empty;
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int rndNum = rnd.Next(0, str.Length - 1);
                result = result + str[rndNum];
            }

            return result;
        }

        public static string SHA256Hash(string data)
        {

            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}