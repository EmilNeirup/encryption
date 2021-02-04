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
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 4 == 0)
                {
                    bytes[i] += (byte)key[0];
                }
                else if (i % 2 == 0)
                {
                    bytes[i] += (byte)key[1];
                }
                else
                {
                    bytes[i] += (byte)key[2];
                }
            }
            return bytes;
        }

        public byte[] SubstitutionDecrypt(byte[] bytes, string key)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 4 == 0)
                {
                    bytes[i] -= (byte)key[0];
                }
                else if (i % 2 == 0)
                {
                    bytes[i] -= (byte)key[1];
                }
                else
                {
                    bytes[i] -= (byte)key[2];
                }
            }
            return bytes;
        }
    }

}