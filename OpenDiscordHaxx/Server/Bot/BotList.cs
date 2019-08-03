using Newtonsoft.Json;
using System.Collections.Generic;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    class BotListItem
    {
        [JsonProperty("at")]
        public string At { get; set; }


        [JsonProperty("id")]
        public ulong Id { get; set; }


        [JsonProperty("verification")]
        public string Verification { get; set; }
    }

    public class BotList : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            List<BotListItem> bots = new List<BotListItem>();
            foreach (var client in Server.Bots)
            {
                BotListItem bot = new BotListItem() { At = client.User.ToString(), Id = client.User.Id };

                if (client.User.TwoFactorAuth)
                    bot.Verification = "Phone verified";
                else if (client.User.EmailVerified)
                    bot.Verification = "Email verified";
                else
                    bot.Verification = "None";

                bots.Add(bot);
            }

            Send(JsonConvert.SerializeObject(bots));
        }
    }
}
