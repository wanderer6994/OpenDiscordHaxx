using System.Threading;
using System.Collections.Specialized;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiscordHaxx
{
    class Program
    {
        static void Main()
        {
            Console.Title = "OpenDiscordHaxx [BETA]";

            Server.OngoingAttacks.CollectionChanged += OngoingAttacks_CollectionChanged;
            SocketServer.Start();
            Server.LoadAccounts();
            Server.ServerStatus = "Ready";

            Thread.Sleep(-1);
        }

        private static void OngoingAttacks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SocketServer.Broadcast("/dashboard", new DashboardRequest<ObservableCollection<Attack>>(DashboardOpcode.AttacksUpdate)
                                                 { Data = Server.OngoingAttacks });
        }
    }
}
