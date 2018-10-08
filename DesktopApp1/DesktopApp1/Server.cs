using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    /*
     * Server Class
     * Abstraccts Creation and Management of Server
     * 
     * */
    class Server
    {
        //Private Instance Variables
        private IPAddress hostAddress;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        private int numClients;

        /*
         * portNum the port to open the server on
         * **/
        public Server(int portNum)
        {
            hostAddress = IPAddress.Loopback;
            serverSocket = new TcpListener(hostAddress, portNum);
            Connect();
        }

        /*
        * address the address to start the server on
        * portNum the port to open the server on
        * */
        public Server(String address, int portNum)
        {
            hostAddress = IPAddress.Parse(address);
            serverSocket = new TcpListener(hostAddress, portNum);
            Connect();
        }
        public Server() : this(8888)
        {
          
        }

        /*
         * Accept a connection from a client
         * @throws Exception if Accept fails
         * @returns if the connection succeeded
         * */
        public bool Connect()
        {
            try {
                clientSocket = serverSocket.AcceptTcpClient();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        /*
         * Manage connection of clients and the delegation to seperate threads
         * */
        public void Run()
        {
            numClients = 0;
            while (true)
            {
                numClients += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(numClients) + " started!");
                ClientHandler client = new ClientHandler();
                client.startClient(clientSocket, numClients);
            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }
}
