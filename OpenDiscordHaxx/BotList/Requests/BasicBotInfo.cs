using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class BasicBotInfo
    {
        [JsonProperty("at")]
        public string At { get; set; }


        [JsonProperty("id")]
        public string Id { get; set; }


        [JsonProperty("hypesquad")]
        public string Hypesquad { get; set; }


        [JsonProperty("verification")]
        public string Verification { get; set; }


        public static BasicBotInfo FromClient(DiscordClient client)
        {
            BasicBotInfo bot = new BasicBotInfo()
            {
                At = client.User.ToString(),
                Id = client.User.Id.ToString(),
                Hypesquad = client.User.Hypesquad.ToString()
            };

            if (client.User.TwoFactorAuth)
                bot.Verification = "Phone verified";
            else if (client.User.EmailVerified)
                bot.Verification = "Email verified";
            else
                bot.Verification = "None or locked";


            return bot;
        }
    }
}
