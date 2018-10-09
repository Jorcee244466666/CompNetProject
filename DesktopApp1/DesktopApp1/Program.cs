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
                Server server = new Server(1234);
                server.Run();
            } else if (choice.Equals("client"))
            {
                Client client = new Client();
                client.Start();
            } else
            {
                throw new Exception("Invalid Input");
            }
        }
    }
}
