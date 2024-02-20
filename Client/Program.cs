using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ASyncSocketClient.Connect();
            Console.WriteLine("Type in data:");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "q")
                {
                    break;
                }
                ASyncSocketClient.SendData(input);
            }

            Console.ReadLine();
        }
    }
}
