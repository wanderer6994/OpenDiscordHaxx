using System;
using Discord;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;

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

                SocketServer.Broadcast(DashboardOpcode.StatusUpdate, 
                                        new StatusUpdate() { Status = _serverStatus });
            }
        }


        #region accounts
        public static List<DiscordClient> Bots = new List<DiscordClient>();

        public static void LoadAccounts()
        {
            ServerStatus = "Loading bots";

            StartAccountBroadcaster();

            string[] tokens = File.ReadAllLines("Tokens.txt");

            foreach (var token in tokens)
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

            if (Bots.Count < tokens.Length)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var bot in Bots)
                    builder.AppendLine(bot.Token);

                File.WriteAllText("Tokens-valid.txt", builder.ToString());
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

                        SocketServer.Broadcast(DashboardOpcode.OverlookUpdate, 
                                                new OverlookUpdate()
                                                {
                                                    Accounts = Bots.Count,
                                                    Attacks = OngoingAttacks.Count
                                                });
                        
                        Thread.Sleep(1100);
                    }
                }
            });
        }
        #endregion


        public static ObservableCollection<Attack> OngoingAttacks = new ObservableCollection<Attack>();
    }
}
