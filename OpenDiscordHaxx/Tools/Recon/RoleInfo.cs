using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class RoleInfo
    {
        public RoleInfo(Role role)
        {
            Name = role.Name;
            Id = role.Id;
        }



        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("id")]
        public ulong Id { get; private set; }
    }
}
