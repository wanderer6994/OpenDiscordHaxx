using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace DiscordHaxx
{
    public static class CommandHandler
    {
        public static void TryHandle(string content)
        {
            IBot bot = null;

            switch (JsonConvert.DeserializeObject<BotRequest>(content).Opcode)
            {
                case BotOpcode.Join:
                    bot = new Joiner(JsonConvert.DeserializeObject<JoinRequest>(content));
                    break;
                case BotOpcode.Leave:
                    bot = new Leaver(JsonConvert.DeserializeObject<LeaveRequest>(content));
                    break;
                case BotOpcode.Flood:
                    bot = new Flooder(JsonConvert.DeserializeObject<FloodRequest>(content));
                    break;
                case BotOpcode.Friend:
                    bot = new Friender(JsonConvert.DeserializeObject<FriendRequest>(content));
                    break;
            }

            Task.Run(() => bot.Start());
        }
    }
}
