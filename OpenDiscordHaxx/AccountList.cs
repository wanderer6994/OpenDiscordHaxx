using Discord;
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
        private List<DiscordClient> _accounts;
        private bool _reloaderRunning;
        private bool _tokensLoading;

        public AccountList()
        {
            _accounts = new List<DiscordClient>();
        }


        public async void LoadAsync()
        {
            await Task.Run(() =>
            {
                SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, _accounts));
                _accounts.Clear();
                Server.ServerStatus = "Loading bots";
                _tokensLoading = true;


                string[] tokens = File.Exists("Tokens.txt") 
                                        ? File.ReadAllLines("Tokens.txt") : new string[] { };


                foreach (var token in tokens.Distinct())
                {
                    try
                    {
                        DiscordClient client = new DiscordClient(token);
                        _accounts.Add(client);
                        SocketServer.Broadcast("/list", new ListRequest(ListAction.Add, client));
                    }
                    catch (DiscordHttpException) { }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Unknown error when loading account:\n{e}");
                    }
                }


                var bruh = _accounts.GroupBy(bot => bot.User.Id);
                List<DiscordClient> removedAccounts = new List<DiscordClient>();
                foreach (var ok in bruh)
                {
                    List<DiscordClient> clients = ok.ToList();
                    clients.RemoveAt(0);

                    removedAccounts.AddRange(clients);
                }

                _accounts = bruh.Select(group => group.First()).ToList();


                if (removedAccounts.Count > 0)
                {
                    foreach (var bot in new List<DiscordClient>(removedAccounts))
                    {
                        if (_accounts.Contains(bot))
                            removedAccounts.RemoveAt(removedAccounts.IndexOf(bot));
                    }

                    SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, removedAccounts));
                }


                if (_accounts.Count < tokens.Length)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var bot in _accounts)
                        builder.AppendLine(bot.Token);

                    File.WriteAllText("Tokens-valid.txt", builder.ToString());
                }


                Server.ServerStatus = "Ready";
                _tokensLoading = false;

                if (!_reloaderRunning)
                    StartAutoReloaderAsync();
            });
        }


        private async void StartAutoReloaderAsync()
        {
            await Task.Run(() =>
            {
                _reloaderRunning = true;

                string currentContent = File.ReadAllText("Tokens.txt");

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


        public static implicit operator List<DiscordClient>(AccountList instance)
        {
            return instance._accounts;
        }
    }
}
