using System.Threading;

namespace DiscordHaxx
{
    class Program
    {
        static void Main()
        {
            SocketServer.Start();
            Server.LoadAccounts();

            Server.ServerStatus = "Ready";

            Thread.Sleep(-1);
        }
    }
}
