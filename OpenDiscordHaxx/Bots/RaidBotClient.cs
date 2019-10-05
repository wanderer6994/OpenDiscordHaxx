using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;

namespace DiscordHaxx
{
    public class RaidBotClient
    {
        public List<Guild> Guilds { get; set; }
        public List<Relationship> Relationships { get; set; }
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
            client.OnUserUpdated += Client_OnUserUpdated;
            client.OnJoinedGuild += Client_OnJoinedGuild;
            client.OnLeftGuild += (c, args) => Guilds.Remove(args.Guild);
            client.OnChannelCreated += (c, args) => BotStorage.AddChannel(args.Channel);
            client.OnEmojisUpdated += (c, args) => BotStorage.AddEmojis(args.Emojis);
            client.OnGuildMemberUpdated += (c, args) => BotStorage.AddUser(args.Member.User);
            client.OnRelationshipAdded += (c, args) => Relationships.Add(args.Relationship);
            client.OnRelationshipRemoved += (c, args) => Relationships.Remove(args.Relationship);
        }


        private void Client_OnJoinedGuild(DiscordSocketClient client, GuildEventArgs args)
        {
            BotStorage.AddEmojis(args.Guild.Emojis);
            try
            {
                BotStorage.AddChannels(args.Guild.GetChannels());
            }
            catch { }

            Guilds.Add(args.Guild);
        }


        private void Client_OnUserUpdated(DiscordSocketClient client, UserEventArgs args)
        {
            if (args.User.Id == client.User.Id)
                BotListEndpoint.UpdateList(ListAction.Update, new RaidBotClient(client));
        }


        public static implicit operator DiscordClient(RaidBotClient instance)
        {
            return instance.Client;
        }


        public override string ToString()
        {
            return Client.User.ToString();
        }
    }
}
