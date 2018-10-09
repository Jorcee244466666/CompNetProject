using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatClient
{
    class Client
    {
        private TcpClient mySocket = new TcpClient();
        private NetworkStream serverStream = default(NetworkStream);
        private IPAddress serverAddress;
        private int portNum;
        string receivedMsg = "";

        private void ReceiveMsg()
        {
            Byte[] receiveStream = new byte[10000];
            while (true)
            {
                serverStream = mySocket.GetStream();
                serverStream.Read(receiveStream, 0, receiveStream.Length);
                receivedMsg = Encoding.ASCII.GetString(receiveStream);
                receivedMsg = receivedMsg.Substring(0, receivedMsg.IndexOf("\n"));
                if (receivedMsg.Contains("#"))
                {
                    Console.WriteLine("#" + receivedMsg);
                    receivedMsg = "";
                }
                receivedMsg = "";

            }
        }

        private void SendMsg()
        {
            Byte[] msgBytes;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    msgBytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                    serverStream.Write(msgBytes, 0, msgBytes.Length);
                    serverStream.Flush();
                }
            }
        }

        public void Start()
        {
            Console.WriteLine("Please Enter Server IP: ");
            serverAddress = IPAddress.Loopback;
            Console.WriteLine("Please Enter the Port Number to use: ");
            portNum = int.Parse(Console.ReadLine());
            mySocket.Connect(serverAddress, portNum);
            Thread receiveThread = new Thread(ReceiveMsg);
            Thread sendThread = new Thread(SendMsg);
            receiveThread.Start();
            receiveThread.IsBackground = true;
            sendThread.Start();
        }
    }
}
