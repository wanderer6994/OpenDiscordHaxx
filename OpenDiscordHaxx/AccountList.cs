using Discord;
using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class AccountList
    {
        public List<RaidBotClient> Accounts { get; private set; }
        private bool _reloaderRunning;
        private bool _tokensLoading;

        public AccountList()
        {
            Accounts = new List<RaidBotClient>();
        }


        public async void LoadAsync()
        {
            await Task.Run(() =>
            {
                SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, Accounts.ToClients()));
                Accounts.Clear();
                Server.ServerStatus = "Loading bots";
                _tokensLoading = true;


                string[] tokens = File.Exists("Tokens.txt") 
                                        ? File.ReadAllLines("Tokens.txt") : new string[] { };


                foreach (var token in tokens.Distinct())
                {
                    try
                    {
                        DiscordClient client = null;

                        if (Accounts.Count < 50)
                        {
                            DiscordSocketClient sClient = new DiscordSocketClient();
                            sClient.OnLoggedIn += Client_OnLoggedIn;
                            sClient.Login(token);
                            Accounts.Add(new RaidBotClient(sClient));
                            client = sClient;
                        }
                        else
                        {
                            client = new DiscordClient(token);
                            Accounts.Add(new RaidBotClient(client));
                        }

                        SocketServer.Broadcast("/list", new ListRequest(ListAction.Add, client));
                    }
                    catch (DiscordHttpException) { }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Unknown error when loading account:\n{e}");
                    }
                }


                var bruh = Accounts.GroupBy(bot => bot.Client.User.Id);
                List<RaidBotClient> removedAccounts = new List<RaidBotClient>();
                foreach (var ok in bruh)
                {
                    List<RaidBotClient> clients = ok.ToList();
                    clients.RemoveAt(0);

                    removedAccounts.AddRange(clients);
                }

                Accounts = bruh.Select(group => group.First()).ToList();


                if (removedAccounts.Count > 0)
                {
                    foreach (var bot in new List<RaidBotClient>(removedAccounts))
                    {
                        if (Accounts.Contains(bot))
                            removedAccounts.RemoveAt(removedAccounts.IndexOf(bot));
                    }

                    SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, removedAccounts.ToClients()));
                }


                if (Accounts.Count < tokens.Length)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var bot in Accounts)
                        builder.AppendLine(bot.Client.Token);

                    File.WriteAllText("Tokens-valid.txt", builder.ToString());
                }


                Server.ServerStatus = "Ready";
                _tokensLoading = false;

                if (!_reloaderRunning)
                    StartAutoReloaderAsync();
            });
        }

        private void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Accounts.First(acc => acc.Client.User.Id == client.User.Id).Guilds = args.Guilds.Cast<Guild>().ToList();
        }

        private async void StartAutoReloaderAsync()
        {
            await Task.Run(() =>
            {
                _reloaderRunning = true;

                string currentContent = null;

                if (File.Exists("Tokens.txt"))
                     currentContent = File.ReadAllText("Tokens.txt");

                while (true)
                {
                    if (File.Exists("Tokens.txt"))
                    {
                        string content = File.ReadAllText("Tokens.txt");

                        if (currentContent != content && !_tokensLoading)
                        {
                            LoadAsync();

                            currentContent = content;
                        }

                        Thread.Sleep(2000);
                    }
                }
            });
        }
    }
}
