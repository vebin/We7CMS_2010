using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace We7.CMS.Accounts
{
    public class CryptoHelper
    {
        public static byte Char2Hex(string ch)
        {
            switch (ch)
            { 
                case "0":
                    return 0x00;
                case "1":
                    return 0x01;
                case "2":
                    return 0x02;
                case "3":
                    return 0x03;
                case "4":
                    return 0x04;
                case "5":
                    return 0x05;
                case "6":
                    return 0x06;
                case "7":
                    return 0x07;
                case "8":
                    return 0x08;
                case "9":
                    return 0x09;
                case "A":
                    return 0x0A;
                case "B":
                    return 0x0B;
                case "C":
                    return 0x0C;
                case "D":
                    return 0x0D;
                case "E":
                    return 0x0E;
                case "F":
                    return 0x0F;
            }
            return 0x00;
        }


        public static string ComputeHashString(string originalStr)
        {
            return ToBase64String(ComputeHash(ConvertStringToByteArray(originalStr)));
        }

        public static byte[] ComputeHash(byte[] buf)
        {
            return SHA1.Create().ComputeHash(buf);
        }

        public static byte[] ConvertStringToByteArray(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] HexStringToByteArray(string s)
        { 
            Byte[] buf = new Byte[s.Length/2];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(Char2Hex(s.Substring(i * 2, 1))*0x10 + Char2Hex(s.Substring(i*2+1, 1)));
            }
            return buf;
        }

        public static string ToBase64String(byte[] buf)
        {
            return System.Convert.ToBase64String(buf);
        }
    }

    public class CryptoService
    {
        string sKey = "22362E7A9285DD53A0BBC2932F9733C505DC04EDBFE00D70";
        string sIV = "1E7FA9231E7FA923";

        byte[] byteKey;
        byte[] byteIV;

        public CryptoService(string key, string iv)
        {
            sKey = key;
            sIV = iv;

            byteKey = CryptoHelper.HexStringToByteArray(sKey);
            byteIV = CryptoHelper.HexStringToByteArray(sIV);
        }

        public bool Encrypt(byte[] toEncrypt, out byte[] encrypted)
        {
            bool success = false;
            encrypted = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, TripleDESCryptoServiceProvider.Create().CreateEncryptor(byteKey, byteIV), CryptoStreamMode.Write))
                {
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                    encrypted = ms.ToArray();
                }
            }
            catch (CryptographicException e)
            {
                success = false;
            }

            return success;
        }
    }
}
