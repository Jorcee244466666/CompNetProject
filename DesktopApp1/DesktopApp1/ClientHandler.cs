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
        public void startClient(TcpClient inClientSocket, int cNum)
        {
            this.clientSocket = inClientSocket;
            this.clientNum = cNum;
            Thread clientThread = new Thread(doChat);
            clientThread.Start();
        }

        private void doChat()
        {
        }
    }
}
