using System;
using System.Net;
using System.Net.Sockets;

using System.Text;

namespace Client
{
    internal class ASyncSocketClient
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Connect()
        {
            int att = 0;
            while (!clientSocket.Connected)
            {
                try
                {
                    att++;
                    clientSocket.Connect(IPAddress.Loopback, 8080);
                }
                catch (SocketException e)
                {
                    Console.Clear();
                    Console.WriteLine($"Attempt no. {att}");
                    //Console.WriteLine(e);
                }

            }
            Console.Clear();
            Console.WriteLine("Connected!");
        }

        public static void SendData(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            clientSocket.Send(buffer);

            byte[] recBuffer = new byte[1024];
            int rec = clientSocket.Receive(recBuffer);
            byte[] recData = new byte[rec];
            Array.Copy(recBuffer, recData, rec);
            Console.WriteLine($"{Encoding.ASCII.GetString(recData)}");
        }

    }
}
