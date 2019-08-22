using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ServerRecon : ReconRequest
    {
        public ServerRecon(int id) : base(id, ReconOpcode.ReconCompleted)
        {
            Roles = new List<RoleInfo>();
        }


        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("region")]
        public string Region { get; set; }


        [JsonProperty("verification")]
        public string VerificationLevel { get; set; }


        [JsonProperty("vanity_invite")]
        public string VanityInvite { get; set; }


        [JsonProperty("bots_in_guild")]
        public string BotsInGuild { get; set; }


        [JsonProperty("roles")]
        public List<RoleInfo> Roles { get; set; }
    }
}
