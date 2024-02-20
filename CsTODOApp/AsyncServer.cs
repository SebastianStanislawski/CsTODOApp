using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class AsyncSocketServer
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static List<Socket> clientSockets = new List<Socket>();

        private static byte[] buffer = new byte[1024];

        public static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            //Socket handler = (Socket)ar.AsyncState;
            Socket socket = serverSocket.EndAccept(ar);
            clientSockets.Add(socket);
            Console.WriteLine($"Someone connected!");
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult ar)
       {
            Socket socket = (Socket)ar.AsyncState;

            int received = socket.EndReceive(ar);
            byte[] dataBuffer = new byte[received];
            Array.Copy(buffer, dataBuffer, received);
            string text = Encoding.ASCII.GetString(dataBuffer);
            Console.WriteLine($"Received: {text}");




            byte[] data = Encoding.ASCII.GetBytes($"{text} from server");
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}
