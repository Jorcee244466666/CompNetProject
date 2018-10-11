using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Start a Server or Client?");
            String choice = Console.ReadLine();
            choice = choice.ToLower();
            if (choice.Equals("server"))
            {
                Console.WriteLine("Please enter the IP to Start the Server on");
                String address = Console.ReadLine();
                Console.WriteLine("Please Enter the Port to start the server on");
                int portNum = int.Parse(Console.ReadLine());
                Server server = new Server(address, portNum);
                server.Run();
            } else if (choice.Equals("client"))
            {
                ClientView client = new ClientView();
                client.ShowDialog();
            } else
            {
                throw new Exception("Invalid Input");
            }
        }
    }
}
