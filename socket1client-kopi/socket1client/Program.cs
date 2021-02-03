using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace socket1client
{
    class Program
    {
        static public void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            int port = 11000;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();

            while (true)
            {
                ReceiveMessage(stream);

                Console.Write("Write your message here: ");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);

                if (text == "c")
                {
                    break;
                }

                byte key = 255;

                for (int i = 0; i < buffer.Length; i++)
                {
                    int change = i + key;
                    while (change + buffer[i] > 255) change -= 256;
                    buffer[i] += (byte)change;
                }

                stream.Write(buffer, 0, buffer.Length);
            }

            client.Close();
        }

        static public async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];

            int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);

            byte key = 255;

            for (int i = 0; i < numberOfBytesRead; i++)
            {
                int change = i + key;
                while (change - buffer[i] < 0) change += 256;
                buffer[i] -= (byte)change;
            }

            string recievedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

            Console.WriteLine(recievedMessage);
        }

    }

}
