using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace DiscordHaxx
{
    public static class CommandHandler
    {
        public static void Handle(HttpListenerContext context)
        {
            string content = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            IBot bot = null;

            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

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
            catch (CheckException e)
            {
                context.Response.StatusCode = 400;

                byte[] contents;
                if (e.Error != null)
                    contents = Encoding.UTF8.GetBytes(e.Error);
                else
                    contents = Encoding.UTF8.GetBytes("Unknown"); //not all bots have check exceptions yet
                context.Response.OutputStream.Write(contents, 0, contents.Length);
            }
            catch (JsonSerializationException)
            {
                context.Response.StatusCode = 400;

                byte[] contents = Encoding.UTF8.GetBytes("Invalid input");
                context.Response.OutputStream.Write(contents, 0, contents.Length);
            }

            context.Response.OutputStream.Close();
        }
    }
}
