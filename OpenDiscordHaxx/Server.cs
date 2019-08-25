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

                SocketServer.Broadcast("/dashboard",
                                        new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate) { Data = new StatusUpdate() { Status = _serverStatus } });
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

                        var update = new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate);
                        update.Data.Accounts = Bots.Count;
                        update.Data.Attacks = OngoingAttacks.Count;

                        SocketServer.Broadcast("/dashboard", update);
                    }

                    Thread.Sleep(1100);
                }
            });
        }


        public static ObservableCollection<Attack> OngoingAttacks = new ObservableCollection<Attack>();
    }
}
