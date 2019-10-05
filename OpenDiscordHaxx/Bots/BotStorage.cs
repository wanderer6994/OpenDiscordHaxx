using Discord;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DiscordHaxx
{
    public static class BotStorage
    {
        public static void Initialize()
        {
            DefaultEmojis = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Emojis.json"));
            CustomEmojis = new List<Emoji>();
            GuildChannels = new List<GuildChannel>();
            Invites = new List<Invite>();
            Users = new List<User>();
        }

        public static Dictionary<string, string> DefaultEmojis { get; private set; }
        public static List<Emoji> CustomEmojis { get; private set; }
        public static List<GuildChannel> GuildChannels { get; private set; }
        public static List<Invite> Invites { get; private set; }
        public static List<User> Users { get; private set; }


        public static void AddEmoji(Emoji emoji)
        {
            CustomEmojis.RemoveAll(e => e.Id == emoji.Id);
            CustomEmojis.Add(emoji);
        }

        public static void AddEmojis(IEnumerable<Emoji> emojis)
        {
            foreach (var emoji in emojis)
                AddEmoji(emoji);
        }


        public static void AddChannel(GuildChannel channel)
        {
            GuildChannels.RemoveAll(c => c.Id == channel.Id);
            GuildChannels.Add(channel);
        }

        public static void AddChannels(IEnumerable<GuildChannel> channels)
        {
            foreach (var channel in channels)
                AddChannel(channel);
        }


        public static void AddInvite(Invite invite)
        {
            Invites.RemoveAll(i => i.Code == invite.Code);
            Invites.Add(invite);
        }


        public static void AddUser(User user)
        {
            Users.RemoveAll(u => u.Id == user.Id);
            Users.Add(user);
        }
    }
}
