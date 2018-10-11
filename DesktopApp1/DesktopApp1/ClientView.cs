using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientView : Form
    {
        private TcpClient mySocket = new TcpClient();
        private IPAddress serverAddress;
        private bool connected = false;
        private int portNum = 0;
        private NetworkStream serverStream;
        string receivedMsg = "";

        public ClientView()
        {
            InitializeComponent();
        }

        private void ClientView_Load(object sender, EventArgs e)
        {
            Msg("Client Started");
            textBox2.Text = "What IP Address would you like to connect to?";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                if (serverAddress == null)
                {
                    try
                    {
                        serverAddress = IPAddress.Parse(textBox1.Text);
                    }
                    catch (Exception ex)
                    {
                        textBox2.Text = ex.ToString();
                        serverAddress = null;
                        return;
                    }
                    textBox2.Text = "What Port would you like to connect with?";
                    textBox1.Text = "";
                    textBox1.Focus();
                }
                else if (portNum == 0)
                {
                    try
                    {
                        portNum = int.Parse(textBox1.Text);
                    }
                    catch (Exception ex)
                    {
                        textBox2.Text = ex.ToString();
                        portNum = 0;
                        return;
                    }
                    textBox1.Text = "";
                    textBox2.Text = "Press Connect to Connect";
                }
                return;
            }
            else
            {
                serverStream = mySocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox1.Text + "#");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                textBox1.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public void Msg(string msg)
        {
            if (msg.Contains("#"))
                msg = msg.Substring(0, msg.IndexOf("#"));
            textBox2.Text = textBox2.Text + Environment.NewLine + " >> " + msg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (portNum > 0 && serverAddress != null)
            {
                try
                {
                    mySocket.Connect(serverAddress, portNum);
                }
                catch (Exception ex)
                {
                    textBox2.Text = ex.ToString();
                    return;
                }
                connected = true;
                button2.Enabled = false;
                label1.Text = "Connected to " + serverAddress;
                textBox2.Text = "";
                Thread receiveThread = new Thread(ReceiveMsg);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
        }
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
                    Msg(receivedMsg);
                    receivedMsg = "";
                }
                receivedMsg = "";

            }
        }
    }
}
