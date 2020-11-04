using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Client
{
    
    class Program
    {
        static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя пользователя");
            string nickname = Console.ReadLine();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8003));
            client.Send(Encoding.UTF8.GetBytes(nickname));
            Task.Run(() => MessageReceive(client));
            while(true)
            {
                string message = Console.ReadLine();
                if (message != "")
                    client.Send(Encoding.UTF8.GetBytes(message));
            }
        }
        static void MessageReceive(Socket s)
        {
            byte[] buffer = new byte[2048];
            int count;
            try
            {
                while (true)
                {
                    count = s.Receive(buffer);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, count));
                }
            }
            catch (SocketException)
            {
                s.Close();
            }
        }
    }
}
