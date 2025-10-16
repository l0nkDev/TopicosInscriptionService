using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace TopicosP1Backend.Scripts
{
    public class Util
    {
        static public string Hash(string s)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                byte[] bytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(s));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
        static public string GenToken() { return string.Join("", Enumerable.Repeat(0, 100).Select(n => (char)new Random().Next(32, 127))).Replace("/", "").Replace(" ", "").Replace("\\", "").Replace("\"", "").Replace("'", ""); }
    }
}
