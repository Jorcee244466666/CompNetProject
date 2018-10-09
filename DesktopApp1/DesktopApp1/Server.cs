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
        private LinkedList<TcpClient> clientSockets = new LinkedList<TcpClient>();
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
                clientSockets.AddLast(serverSocket.AcceptTcpClient());
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            numClients++;
            return true;
        }

        /*
         * Manage connection of clients and the delegation to seperate threads
         * */
        public void Run()
        {
            LinkedList<ClientHandler> clients = new LinkedList<ClientHandler>();
            numClients = 0;
            while (numClients >= 0)
            {
                Connect();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(numClients) + " started!");
                Broadcast(PrepareMsg(Convert.ToString(numClients), "", false));
                clients.AddLast(new ClientHandler());
                clients.Last().StartClient(clientSockets.Last(), numClients);
            }
            foreach (TcpClient i in clientSockets)
            {
                i.Close();
            }
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
        /*
         * Broadcast received messages to all clients
         * */
        public static void Broadcast(Byte[] msg)
        {
            NetworkStream bStream;
            foreach (TcpClient client in clientSockets)
            {
                bStream = client.GetStream();
                bStream.Write(msg, 0, msg.Length);
                bStream.Flush();
            }

        }
        /*
         * Prepare a message to be sent on a Network Stream
         * user the name of the user who is sending the message
         * msg the message the user wants to send
         * isMsg whether the message is a message (alternative being join notification
         * */
        public static Byte[] PrepareMsg(String user, String msg, bool isMsg)
        {
            Byte[] bBytes = null;
            if (isMsg)
            {
                bBytes = Encoding.ASCII.GetBytes(user + ": " + msg + "\n");
            }
            else
            {
                bBytes = Encoding.ASCII.GetBytes(user + " joined the chat\n");
            }
            return (bBytes);
        }
    }
}
