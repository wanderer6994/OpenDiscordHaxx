using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using Discord;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;

namespace DiscordHaxx
{
    public class CheckerEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Task.Run(() =>
            {
                Send(JsonConvert.SerializeObject(new CheckerStartedRequest(CheckerOpcode.Started)));

                int valid = 0;
                int invalid = 0;
                int total = Server.Bots.Count;
                foreach (var client in new List<DiscordClient>(Server.Bots))
                {
                    BotCheckedRequest req = new BotCheckedRequest(CheckerOpcode.BotChecked)
                                                        { Bot = BotInfo.FromClient(client) };
                    req.Progress.Total = total;

                    try
                    {
                        client.JoinGuild("a");
                    }
                    catch (DiscordHttpException e)
                    {
                        req.Valid = e.Code == DiscordError.UnknownInvite;
                    }
                    catch (JsonReaderException)
                    {
                        Send(JsonConvert.SerializeObject(new CheckerRequest(CheckerOpcode.RateLimited)));

                        break;
                    }


                    if (req.Valid)
                        valid++;
                    else
                    {
                        Server.Bots.Remove(client);
                        invalid++;
                    }

                    req.Progress.Valid = valid;
                    req.Progress.Invalid = invalid;


                    Send(JsonConvert.SerializeObject(req));
                }

                if (total > Server.Bots.Count)
                {
                    StringBuilder tokensToAdd = new StringBuilder();
                    foreach (var bot in Server.Bots)
                        tokensToAdd.AppendLine(bot.Token);

                    File.WriteAllText("Tokens-checked.txt", tokensToAdd.ToString());
                }

                Send(JsonConvert.SerializeObject(new CheckerRequest(CheckerOpcode.Done)));
            });
        }
    }
}
