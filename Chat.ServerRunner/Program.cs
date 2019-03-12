using Chat.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.ServerRunner
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            var server = new ChatServer();
            server.Start();

            Console.WriteLine("Service started.");
            Console.ReadLine();

            server.Stop();
        }
    }
}
