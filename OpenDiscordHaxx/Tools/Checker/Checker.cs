using Discord;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Checker
    {
        public CheckerProgress Progress { get; private set; }

        public bool Finished { get; private set; }


        public Checker()
        {
            Progress = new CheckerProgress
            {   Total = Server.Bots.Count   };
        }


        public async void StartAsync()
        {
            await Task.Run(() =>
            {
                if (Server.Bots.Count == 0)
                {
                    SocketServer.Broadcast("/checker", new CheckerErrorRequest("notokens"));
                    Finished = true;
                    return;
                }

                SocketServer.Broadcast("/checker", new CheckerStartedRequest());

                foreach (var client in new List<DiscordClient>(Server.Bots))
                {
                    BotCheckedRequest req = new BotCheckedRequest(client);

                    try
                    {
                        client.JoinGuild("a");
                    }
                    catch (DiscordHttpException e)
                    {
                        req.Valid = e.Code == DiscordError.UnknownInvite || e.Code == DiscordError.MaximumGuilds;
                    }
                    catch (JsonReaderException)
                    {
                        SocketServer.Broadcast("/checker", new CheckerErrorRequest("ratelimit"));
                        Finished = true;
                        break;
                    }


                    if (req.Valid)
                        Progress.Valid++;
                    else
                    {
                        Server.Bots.Remove(client);
                        SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, client));
                        Progress.Invalid++;
                    }

                    req.Progress = Progress;

                    SocketServer.Broadcast("/checker", req);
                }

                if (Progress.Total > Server.Bots.Count)
                {
                    StringBuilder tokensToAdd = new StringBuilder();
                    foreach (var bot in Server.Bots)
                        tokensToAdd.AppendLine(bot.Token);

                    File.WriteAllText("Tokens-checked.txt", tokensToAdd.ToString());
                }

                SocketServer.Broadcast("/checker", new CheckerRequest(CheckerOpcode.Done));

                Finished = true;
            });
        }
    }
}
