using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using cryptofar;

namespace socket1Server
{
    class Program
    {
        static public long a = 167823467234;
        static public string key = "";

        static public KeyExchange KeyExchange = new KeyExchange();
        static public PrivateKeyEncryption encryption = new PrivateKeyEncryption();
        static public List<TcpClient> clients = new List<TcpClient>();
        static public void Main(string[] args)
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 11000;
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            AcceptClients(listener);

            bool isRunning = true;
            while(isRunning)
            {
                Console.Write("Write message: ");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);


                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(encryption.SubstitutionEncrypt(buffer, key), 0, buffer.Length);
                }
            }
        }

        static public async void AcceptClients(TcpListener listener)
        {
            bool isRunning = true;
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();

                byte[] keybuffer = BigInteger.ModPow(KeyExchange.g, a, KeyExchange.n).ToByteArray();

                stream.Write(keybuffer);

                byte[] buffer = new byte[256];

                int nBytesRead = await stream.ReadAsync(buffer);

                key = KeyExchange.GenerateKey(new BigInteger(buffer), a);

                ReceiveMessages(stream);
            }
        }

        static public async void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[256];

            bool isRunning = true;
            while (isRunning)
            {
                int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                string text = Encoding.UTF8.GetString(encryption.SubstitutionDecrypt(buffer, key), 0, read);
                Console.WriteLine("Client writes: " + text);
            }
        }
    }
}