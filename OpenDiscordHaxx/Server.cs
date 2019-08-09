using System;
using Discord;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

                var req = new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate);
                req.Data.Status = _serverStatus;
                SocketServer.Broadcast("/dashboard", req);
            }
        }


        #region accounts
        public static List<DiscordClient> Bots = new List<DiscordClient>();

        public static void LoadAccounts()
        {
            ServerStatus = "Loading bots";

            StartAccountBroadcaster();

            foreach (var token in File.ReadAllLines("Tokens.txt"))
            {
                try
                {
                    Bots.Add(new DiscordClient(token));
                }
                catch (DiscordHttpException) { }
                catch (Exception e)
                {
                    Console.WriteLine($"Unknown error when loading account:\n{e}");
                }
            }
        }


        private static async void StartAccountBroadcaster()
        {
            await Task.Run(() =>
            {
                int previousAmount = 0;

                while (true)
                {
                    if (Bots.Count != previousAmount)
                    {
                        previousAmount = Bots.Count;

                        var req = new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate);
                        req.Data.Accounts = Bots.Count;
                        req.Data.Attacks = OngoingAttacks;
                        SocketServer.Broadcast("/dashboard", req);

                        Thread.Sleep(1000);
                    }
                }
            });
        }
        #endregion


        private static int _attacks;
        public static int OngoingAttacks
        {
            get { return _attacks; }
            set
            {
                _attacks = value;

                var req = new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate);
                req.Data.Accounts = Bots.Count;
                req.Data.Attacks = OngoingAttacks;
                SocketServer.Broadcast("/dashboard", req);
            }
        }
    }
}
