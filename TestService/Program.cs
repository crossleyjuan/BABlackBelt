using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerLib;

namespace TestService
{
    class Program
    {
        static void Main(string[] args)
        {

            ChatServer.StartListener(9000);

            Console.WriteLine("Listeing on port: 19000. Press ENTER to shutdown");
            Console.ReadLine();

        }
    }
}
