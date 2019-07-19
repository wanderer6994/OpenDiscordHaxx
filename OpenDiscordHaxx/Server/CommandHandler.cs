using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public static class CommandHandler
    {
        public static void Handle(HttpListenerContext context)
        {
            string content = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            IBot bot = null;

            try
            {
                switch (JsonConvert.DeserializeObject<Request>(content).Opcode)
                {
                    case Opcode.Join:
                        bot = new Joiner(JsonConvert.DeserializeObject<JoinRequest>(content));
                        break;
                    case Opcode.Leave:
                        bot = new Leaver(JsonConvert.DeserializeObject<LeaveRequest>(content));
                        break;
                    case Opcode.Flood:
                        bot = new Flooder(JsonConvert.DeserializeObject<FloodRequest>(content));
                        break;
                    case Opcode.Friend:
                        bot = new Friender(JsonConvert.DeserializeObject<FriendRequest>(content));
                        break;
                }

                Task.Run(() => bot.Start());

                context.Response.StatusCode = 200;
            }
            catch
            {
                context.Response.StatusCode = 400;
            }

            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.OutputStream.Close();
        }
    }
}
