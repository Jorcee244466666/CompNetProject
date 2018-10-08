using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    class Server
    {
        //Private Instance Variables
        private IPAddress hostAddress;
        private TcpListener serverSocket;
        private TcpClient clientSocket;

        public Server(String address, int portNum)
        {
            hostAddress = IPAddress.Parse(address);
            serverSocket = new TcpListener(hostAddress, portNum);
            clientSocket = default(TcpClient);
            clientSocket = serverSocket.AcceptTcpClient();
        }
        public Server()
        {
            hostAddress = IPAddress.Loopback;
            serverSocket = new TcpListener(hostAddress, 8888);
            clientSocket = default(TcpClient);
            clientSocket = serverSocket.AcceptTcpClient();
        }
    }
}
