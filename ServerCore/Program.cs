using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //DNS (Domain Name System)
            // www.google.com -> 172.1.2.3
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                listenSocket.Bind(endPoint);

                // backlog : 최대 대기수
                listenSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Listening...");

                    //손님을 입장
                    Socket clientSocket = listenSocket.Accept();

                    //받는다
                    byte[] recvBuff = new byte[1024];
                    int recvSize = clientSocket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvSize);

                    Console.WriteLine($"[From Client] {recvData}");

                    //보낸다
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome To Server !");
                    clientSocket.Send(sendBuff);

                    //종료.
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
