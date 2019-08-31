using Discord;
using System.Collections.Generic;

namespace DiscordHaxx
{
    public static class Utils
    {
        public static DiscordClient ToClient(this RaidBotClient client)
        {
            return client;
        }


        public static List<DiscordClient> ToClients(this List<RaidBotClient> clients)
        {
            List<DiscordClient> converted = new List<DiscordClient>();

            foreach (var client in clients)
                converted.Add(client);

            return converted;
        }
    }
}
