using System;
using System.Numerics;

namespace cryptofar
{
    public class KeyExchange
    {
        public int n = 1234;
        public int g = 2;

        public string GenerateKey (BigInteger receivedKey, long ownKey)
        {
            var key = BigInteger.ModPow(receivedKey, ownKey, n);
            return key.ToString();
        }
    }
}