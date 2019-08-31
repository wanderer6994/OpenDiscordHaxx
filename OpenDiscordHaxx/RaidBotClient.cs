using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Gateway;

namespace DiscordHaxx
{
    public class RaidBotClient
    {
        public List<Guild> Guilds { get; set; }
        public DiscordClient Client { get; private set; }
        public bool SocketClient { get; private set; }

        public RaidBotClient(DiscordClient client)
        {
            Client = client;
        }


        public RaidBotClient(DiscordSocketClient client)
        {
            Client = client;
            SocketClient = true;
            client.OnJoinedGuild += Client_OnJoinedGuild;
            client.OnLeftGuild += Client_OnLeftGuild;
            client.OnUserUpdated += Client_OnUserUpdated;
        }

        private void Client_OnUserUpdated(DiscordSocketClient client, UserEventArgs args)
        {
            if (args.User.Id == client.User.Id)
                SocketServer.Broadcast("/list", new ListRequest(ListAction.Update, client));
        }

        private void Client_OnJoinedGuild(DiscordSocketClient client, GuildEventArgs args)
        {
            Guilds.Add(args.Guild);
        }

        private void Client_OnLeftGuild(DiscordSocketClient client, GuildEventArgs args)
        {
            Guilds.Remove(args.Guild);
        }


        public static implicit operator DiscordClient(RaidBotClient instance)
        {
            return instance.Client;
        }
    }
}
