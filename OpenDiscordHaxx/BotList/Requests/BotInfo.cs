using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiscordHaxx
{
    public class BotInfo : BotRequest
    {
        public BotInfo() : base(ListOpcode.BotInfo)
        {
            Badges = new List<string>();
        }

        [JsonProperty("at")]
        public string At { get; private set; }


        [JsonProperty("avatar_id")]
        public string AvatarId { get; private set; }


        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("badges")]
        public List<string> Badges { get; private set; }


        [JsonProperty("verification")]
        public string Verification { get; set; }


        public static BotInfo FromClient(DiscordClient client)
        {
            BotInfo info = new BotInfo
            {
                At = client.User.ToString(),
                Id = client.User.Id.ToString(),
                AvatarId = client.User.AvatarId
            };

            if (client.User.TwoFactorAuth)
                info.Verification = "Phone verified";
            else if (client.User.EmailVerified)
                info.Verification = "Email verified";
            else
                info.Verification = "None or locked";

            foreach (Enum value in Enum.GetValues(typeof(Badge)))
            {
                if (value.ToString() == "LocalUser" || value.ToString() == "None")
                    continue;

                if (client.User.Badges.HasFlag(value))
                    info.Badges.Add(value.ToString());
            }

            if (client.User.Nitro > NitroType.None)
                info.Badges.Add("Nitro");

            return info;
        }
    }
}
