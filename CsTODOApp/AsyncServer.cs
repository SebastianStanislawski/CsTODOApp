using CsTODOApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

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

            string response = string.Empty;
            Database database = new Database();
            switch (text)
            {
                case string s when s.StartsWith("<insert>"):
                    

                    string message = text.Replace("<insert>", "");
                    Console.WriteLine(message);

                    if (message != "")
                    {
                        response = "inserting to the database...";
                        database.Insert(message);
                    }
                    else
                    {
                        response = "Type in message you want to add to the TODO list.";
                    }
                    break;

                case "<update>":
                    response = "updating rows in database...";
                    database.Update();
                    break;

                case "<delete>":
                    response = "deleting rows in database...";
                    database.Delete();
                    break;

                case string s when s.StartsWith("<select>"):
                    List<string>[] list = new List<string>[3];
                    int number = 100;
                    try
                    {
                        string temp = Regex.Match(text, @"\d+", RegexOptions.RightToLeft).Value;
                        if (temp != "")
                        {
                            number = Int16.Parse(temp);
                        }
                        response = $"selecting {number} from database...\n";
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        response = "You tried to select to many rows. Showing 100";
                    }
                    
                    list = database.Select(number);
                    response = "|\tid\t|\tname\t|\tage\t|\n";

                    for (int i = 0; i < list[0].Count; i++)
                    {
                        for (int j = 0; j < list.Length; j++)
                        {
                            response += $"|\t{list[j][i]}\t";
                        }
                        response += "|\n";
                    }

                    break;

                default:
                    response = "command not found!";
                    break;
            }

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}
