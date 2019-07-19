using System.Collections.Generic;
using System.IO;
using Discord;

namespace DiscordHaxx
{
    class Program
    {
        public static List<DiscordClient> Bots { get; private set; }


        static void Main()
        {
            Bots = new List<DiscordClient>();

            foreach (var token in File.ReadAllLines("Tokens.txt"))
                Bots.Add(new DiscordClient(token));

            Server.Start();
        }
    }
}
