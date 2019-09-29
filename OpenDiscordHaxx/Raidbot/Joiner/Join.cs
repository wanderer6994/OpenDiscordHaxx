using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class JoinRequest : RaidRequest
    {
        [JsonProperty("invite")]
        public string Invite { get; private set; }


        [JsonProperty("anti_track")]
        public bool EnableAntiTrack { get; private set; }
    }
}
