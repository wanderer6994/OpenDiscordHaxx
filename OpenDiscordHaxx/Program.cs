using System.Collections.Generic;
using System.IO;
using System.Threading;
using Discord;

namespace DiscordHaxx
{
    class Program
    {
        static void Main()
        {
            Server.ServerStatus = "Loading bots";
            SocketServer.Start();
            Server.LoadAccounts();

            Server.ServerStatus = "Online";

            //HttpServer.Start();

            Thread.Sleep(-1);
        }
    }
}
