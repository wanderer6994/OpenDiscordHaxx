using System.Threading;

namespace DiscordHaxx
{
    class Program
    {
        static void Main()
        {
            Server.ServerStatus = "Loading bots";
            SocketServer.Start();
            Server.LoadAccounts();

            Server.ServerStatus = "Ready";

            Thread.Sleep(-1);
        }
    }
}
