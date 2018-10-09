using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    /*
     * Handle Communication between a client and the server on separate thread
     * */
    class ClientHandler
    {
        TcpClient clientSocket;
        int clientNum;

        /*
         * Start the client onto it's own thread
         * inClientSocket the Client Connected Socket
         * cNum the assigned number of the client
         * */
        public void StartClient(TcpClient inClientSocket, int cNum)
        {
            this.clientSocket = inClientSocket;
            this.clientNum = cNum;
            Thread clientThread = new Thread(DoChat);
            clientThread.Start();
        }

        private void DoChat()
        {
            Byte[] clientBytes = new byte[10000];
            Byte[] sendBytes;
            String clientMsg;
            
            while(true)
            {
                try
                {
                    NetworkStream clientStream = clientSocket.GetStream();
                    clientStream.Read(clientBytes, 0, clientBytes.Length);
                    clientMsg = Encoding.ASCII.GetString(clientBytes);
                    clientMsg = clientMsg.Substring(0, clientMsg.IndexOf("#"));
                    Console.WriteLine("Received from Client: " + clientMsg);
                    sendBytes = Server.PrepareMsg(Convert.ToString(clientNum), clientMsg, true);
                    Server.Broadcast(sendBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
