using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public static class Raidbot
    {
        public static string TokenPath { get; set; }
        public static List<DiscordClient> Clients = new List<DiscordClient>();

        public static void LoadClients(string[] tokens)
        {
            Console.WriteLine($"Loading {tokens.Length} accounts");

            Parallel.ForEach(tokens, token =>
            {
                try
                {
                    Clients.Add(new DiscordClient(token));

                    Console.WriteLine("Client added");

                    Program.UpdateTitle();
                }
                catch (DiscordHttpException)
                { }
            });
        }


        public static void MassJoin(string inviteCode)
        {
            Parallel.ForEach(Clients, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, client =>
            {
                try
                {
                    client.JoinGuild(inviteCode);
                }
                catch (DiscordHttpException)
                {
                    Console.WriteLine($"Error from {client.User}");
                }
            });
        }


        public static void Flood(long channelId, string message, bool addAscii)
        {
            while (true)
            {
                Parallel.ForEach(Clients, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, client =>
                {
                    try
                    {
                        client.SendMessage(channelId, message + (addAscii ? GenerateAscii(2000 - message.Length) : ""));
                    }
                    catch (RateLimitException)
                    {
                        Thread.Sleep(10);
                    }
                    catch (DiscordHttpException)
                    {
                        Console.WriteLine($"Error from {client.User}");
                    }
                });
            }
        }

        private static string GenerateAscii(int length)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
                builder.Append((char)random.Next(32, 1114111));
            return builder.ToString();
        }


        public static void MassLeave(long guildId)
        {
            Parallel.ForEach(Clients, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, client =>
            {
                try
                {
                    client.LeaveGuild(guildId);
                }
                catch (DiscordHttpException e)
                {
                    if (e.Error.Code == DiscordError.UnknownGuild)
                        Console.WriteLine($"{client.User} is not in {guildId}");
                    else
                        Console.WriteLine($"Error from {client.User}");
                }
            });
        }


        public static void FriendSpam(string username, int discriminator)
        {
            Parallel.ForEach(Clients, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, client =>
            {
                try
                {
                    client.SendFriendRequest(username, discriminator);
                }
                catch (DiscordHttpException)
                {
                    Console.WriteLine($"Error from {client.User}");
                }
            });
        }
    }
}