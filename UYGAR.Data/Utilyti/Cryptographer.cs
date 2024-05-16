using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UYGAR.Data.Utilyti
{
    public class Cryptographer
    {
        private static byte[] anahtar = {150, 144, 24 , 74 , 174, 177, 195, 132, 76 , 31 , 152, 237, 46 , 55 , 228, 153,
											10 , 204, 131, 56 , 75 , 150, 167, 158, 240, 252, 150, 37 , 3	 , 121, 3  , 239};
        private static byte[] vektor = { 154, 183, 40, 219, 89, 45, 11, 254, 217, 167, 47, 79, 253, 49, 215, 215 };

        public static string Encryption(string original)
        {

            System.Text.ASCIIEncoding textConverter = new System.Text.ASCIIEncoding();
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                byte[] encrypted;
                byte[] toEncrypt;
                byte[] key;
                byte[] IV;
                key = anahtar;
                IV = vektor;
                ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        toEncrypt = textConverter.GetBytes(original);
                        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted).Replace("+", "Element");
                }
            }
        }public static string Decryption(string encryptedstr)
        {
            if (string.IsNullOrEmpty(encryptedstr))
                return "";
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                System.Text.ASCIIEncoding textConverter = new System.Text.ASCIIEncoding();
                byte[] encrypted;
                byte[] fromEncrypt;
                string roundtrip;
                encrypted = Convert.FromBase64String(encryptedstr.Replace("Element", "+"));

                ICryptoTransform decryptor = myRijndael.CreateDecryptor(anahtar, vektor);
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        fromEncrypt = new byte[encrypted.Length];
                        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                        roundtrip = textConverter.GetString(fromEncrypt);
                        return roundtrip.TrimEnd('\0');
                    }
                }
            }
        }

        public static string HashMD5(string orginal)
        {

            Byte[] data1ToHash = (new UnicodeEncoding()).GetBytes(orginal);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);
            return BitConverter.ToString(hashvalue1);
        }

        public static bool HashCompare(string hashString1, string hashString2)
        {
            Byte[] hashvalue1 = (new UnicodeEncoding()).GetBytes(hashString1);
            Byte[] hashvalue2 = (new UnicodeEncoding()).GetBytes(hashString2);
            int i = 0;
            bool same = true;
            while (i < hashvalue1.Length)
            {
                if (hashvalue1[i] != hashvalue2[i])
                {
                    same = false;
                    break;
                }
                i++;
            }

            if (same)
                return true;
            else
                return false;
        }

    }
}
