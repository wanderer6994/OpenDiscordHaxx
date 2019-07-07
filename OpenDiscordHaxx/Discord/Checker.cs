using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Discord;

namespace DiscordHaxx
{
    public static class Checker
    {
        public static void Check()
        {
            int alive = 0;
            int dead = 0;
            List<DiscordClient> clients = new List<DiscordClient>(Raidbot.Clients);

            foreach (var client in clients)
            {
                if (CheckAccount(client))
                    alive++;
                else
                {
                    Raidbot.Clients.Remove(client);
                    dead++;
                }

                Console.Clear();
                Console.WriteLine($"Alive: {alive} | Dead: {dead}");
                Thread.Sleep(100);
            }

            List<string> tokens = new List<string>();
            Raidbot.Clients.ForEach(client => tokens.Add(client.Token));

            File.WriteAllLines(Raidbot.TokenPath, tokens);

            Program.UpdateTitle();
        }

        private static bool CheckAccount(DiscordClient client)
        {
            try
            {
                client.JoinGuild("a");
            }
            catch (DiscordHttpException e)
            {
                if ((int)e.Error.Code == 40002)
                {
                    Raidbot.Clients.Remove(client);
                    return false;
                }
            }

            return true;
        }
    }
}
