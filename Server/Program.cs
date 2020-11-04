
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static TcpListener server= new TcpListener(IPAddress.Parse("127.0.0.1"),8003);
        static Dictionary<Socket, String> sockets = new Dictionary<Socket, string>();
        static void Main(string[] args)
        {
            server.Start();
            byte[] buffer = new byte[2048];
            int count;
            while (true)
            {
                Socket s = server.AcceptSocket();
                count = s.Receive(buffer);
                string nickname = Encoding.UTF8.GetString(buffer, 0, count);
                foreach (Socket u in sockets.Keys)
                    u.Send(Encoding.UTF8.GetBytes(nickname +" Подключился к чату"));
                sockets.Add(s,nickname);
                Task.Run(() => MessageReceive(s));
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
                    string message = sockets[s] + ": " + Encoding.UTF8.GetString(buffer, 0, count);
                    foreach (Socket u in sockets.Keys)
                        u.Send(Encoding.UTF8.GetBytes(message));
                }
            }
            catch (SocketException)
            {
                sockets.Remove(s);
            }
        }
    }
}
