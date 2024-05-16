using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace UYGAR.Service.Client
{
    public static class Compression
    {


        public static string Compress(string uncompressedString)
        {
            byte[] bytData = Encoding.Unicode.GetBytes(uncompressedString);
            using (MemoryStream ms = new MemoryStream())
            {
                Stream s = new GZipStream(ms, CompressionMode.Compress, true);
                s.Write(bytData, 0, bytData.Length);
                s.Close();
                byte[] compressedData = ms.ToArray();
                return Convert.ToBase64String(compressedData, 0, compressedData.Length);
            }
        }

        public static string DeCompress(string compressedString)
        {
            StringBuilder uncompressedStringBuilder = new StringBuilder();
            byte[] bytInput = System.Convert.FromBase64String(compressedString);
            byte[] writeData = new byte[4096];
            using (Stream zippedStream = new GZipStream(new MemoryStream(bytInput), CompressionMode.Decompress))
            {
                while (true)
                {
                    int size = zippedStream.Read(writeData, 0, writeData.Length);
                    if (size > 0)
                        uncompressedStringBuilder.Append(Encoding.Unicode.GetString(writeData, 0, size));
                    else
                        break;
                }
                zippedStream.Close();
            }
            return uncompressedStringBuilder.ToString();
        }
    }
}
