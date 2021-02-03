using System;
using System.Collections.Generic;
using System.Text;

namespace cryptofar
{
    public class PrivateKeyEncryption
    {

        public byte[] EncryptByte(byte[] bytes, byte key = 255)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                int change = i + key;
                while (change + bytes[i] > 255) change -= 256;
                bytes[i] += (byte)change;
            }
            return bytes;
        }

        public byte[] DecryptByte(byte[] bytes, byte key = 255)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                int change = i + key;
                while (change - bytes[i] < 0) change += 256;
                bytes[i] -= (byte)change;
            }
            return bytes;
        }

        public byte[] SubstitutionEncrypt(byte[] bytes, string key)
        {
            string text = Encoding.UTF8.GetString(bytes);

            char[] chars = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    chars[i] = ' ';
                }
                else
                {
                    int j = bytes[i] - 97;
                    chars[i] = key[j];
                }
            }

            return Encoding.UTF8.GetBytes(chars);
        }

        public byte[] SubstitutionDecrypt(byte[] bytes, string key)
        {
            string text = Encoding.UTF8.GetString(bytes);
            char[] chars = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    chars[i] = ' ';
                }
                else
                {
                    int j = key.IndexOf(text[i]) - 97;
                    chars[i] = (char)j;
                }
            }

            return Encoding.UTF8.GetBytes(chars);
        }
    }

}