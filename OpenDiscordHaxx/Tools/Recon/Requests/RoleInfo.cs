using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class RoleInfo
    {
        public RoleInfo(Role role)
        {
            _name = role.Name;
            _id = role.Id.ToString();
        }



#pragma warning disable IDE0052
        [JsonProperty("name")]
        private readonly string _name;


        [JsonProperty("id")]
        private readonly string _id;
#pragma warning restore IDE0052
    }
}
