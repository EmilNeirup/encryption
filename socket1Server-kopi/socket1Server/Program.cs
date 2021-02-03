using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace socket1Server
{
    class Program
    {
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

                byte key = 255;

                for (int i = 0; i < buffer.Length; i++)
                {
                    int change = i + key;
                    while (change + buffer[i] > 255) change -= 256;
                    buffer[i] += (byte)change;
                }

                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(buffer, 0, buffer.Length);
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

                byte key = 255;

                for (int i = 0; i < buffer.Length; i++)
                {
                    int change = i + key;
                    while (change - buffer[i] < 0) change += 256;
                    buffer[i] -= (byte)change;
                }

                string text = Encoding.UTF8.GetString(buffer, 0, read);
                Console.WriteLine("Client writes: " + text);
            }
        }
    }
}
