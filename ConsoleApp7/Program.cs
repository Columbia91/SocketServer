using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                string srvAddress = "0.0.0.0";
                //string srvAddress = "127.0.0.1";
                //string srvAddress = "10.1.4.1";
                int srvPort = 12345;

                socketServer.Bind(new IPEndPoint(IPAddress.Parse(srvAddress), srvPort));
                socketServer.Listen(100);
                while (true)
                {
                    Socket client = socketServer.Accept();
                    Console.WriteLine("Клиент подключен: ");
                    Console.WriteLine(client.RemoteEndPoint);
                    ThreadPool.QueueUserWorkItem(ClientThreadProc, client);
                }
                    

            }

            //Console.ReadLine();
        }

        private static void ClientThreadProc(object state)
        {
            // протокол работы сервера
            Socket client = state as Socket;
            try
            {
                byte[] buf = new byte[1024];
                while (true)
                {
                    int recSize = client.Receive(buf);
                    Console.WriteLine("recSize = {0}", recSize);
                    Console.WriteLine(Encoding.UTF8.GetString(buf, 0, recSize));

                    client.Send(buf, recSize, SocketFlags.None);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                client.Shutdown(SocketShutdown.Both);
                client.Close();
        }
    }
}
