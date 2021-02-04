using System;
using System.Net;
using System.Numerics;
using System.Net.Sockets;
using System.Text;
using cryptofar;

namespace socket1client
{
    class Program
    {
        static public long b = 7897893456;
        static public string key = "";

        static public KeyExchange keyExchange = new KeyExchange();
        static public PrivateKeyEncryption encryption = new PrivateKeyEncryption();
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            int port = 11000;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[256];

            int nBytesRead = stream.Read(buffer);

            byte[] keyBuffer = BigInteger.ModPow(keyExchange.g, b, keyExchange.n).ToByteArray();

            stream.Write(keyBuffer);

            byte[] temp = new byte[nBytesRead];

            Array.Copy(buffer, 0, temp, 0, nBytesRead);

            key = keyExchange.GenerateKey(new BigInteger(buffer), b);

            while (true)
            {
                ReceiveMessage(stream);
                Console.Write("Write your message here: ");
                string text = Console.ReadLine();

                if (text.ToLower() == "c")
                {
                    break;
                }

                buffer = Encoding.UTF8.GetBytes(text);

                stream.Write(encryption.SubstitutionEncrypt(buffer, key), 0, buffer.Length);
            }
            client.Close();
        }

        static public async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];
            int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
            string recievedMessage = Encoding.UTF8.GetString(encryption.SubstitutionDecrypt(buffer, key), 0, numberOfBytesRead);
            Console.WriteLine("Server writes: " + recievedMessage);
        }
    }
}
