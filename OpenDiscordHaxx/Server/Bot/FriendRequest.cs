﻿using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FriendRequest : BotRequest
    {
        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("discriminator")]
        public uint Discriminator { get; private set; }
    }
}