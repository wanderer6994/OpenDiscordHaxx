using Discord;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DiscordHaxx
{
    public static class Server
    {
        private static string _serverStatus;
        public static string ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;

                SocketServer.Broadcast("/dashboard", new DashboardRequest<StatusUpdate>());
            }
        }


        public static AccountList AccountList = new AccountList();
        public static List<DiscordClient> Bots
        {
            get { return AccountList; }
        }

        public static async void StartAccountBroadcasterAsync()
        {
            await Task.Run(() =>
            {
                int previousAmount = 0;

                while (true)
                {
                    if (Bots.Count != previousAmount)
                    {
                        previousAmount = Bots.Count;

                        SocketServer.Broadcast("/dashboard", new DashboardRequest<OverlookUpdate>());
                    }

                    Thread.Sleep(1100);
                }
            });
        }


        public static ObservableCollection<Attack> OngoingAttacks = new ObservableCollection<Attack>();
    }
}
