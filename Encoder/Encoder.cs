using System;
using System.Text;

namespace LivelyWall.Encoder

{
    public class Encoder
    {
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
