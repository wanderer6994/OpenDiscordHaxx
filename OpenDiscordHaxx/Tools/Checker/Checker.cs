using Discord;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
                Directory.CreateDirectory("Checker-results");
                foreach (var client in new List<RaidBotClient>(Server.Bots))
                {
                    BotCheckedRequest req = new BotCheckedRequest(client);

                    try
                    {
                        client.Client.JoinGuild("a");
                    }
                    catch (DiscordHttpException e)
                    {
                        if (e.Code == DiscordError.UnknownInvite || e.Code == DiscordError.MaximumGuilds)
                            req.Valid = true;
                        else
                        {
                            if (e.Code == DiscordError.AccountUnverified)
                                File.AppendAllText("Checker-results/Locked.txt", client.Client.Token + "\n");
                            else
                                File.AppendAllText("Checker-results/Invalid.txt", client.Client.Token + "\n");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        SocketServer.Broadcast("/checker", new CheckerErrorRequest("ratelimit"));
                        Finished = true;
                        break;
                    }


                    if (req.Valid)
                    {
                        File.AppendAllText("Checker-results/Valid.txt", client.Client.Token + "\n");

                        Progress.Valid++;
                    }
                    else
                    {
                        Server.Bots.Remove(client);
                        SocketServer.Broadcast("/list", new ListRequest(ListAction.Remove, client));
                        Progress.Invalid++;
                    }

                    req.Progress = Progress;

                    SocketServer.Broadcast("/checker", req);
                }

                SocketServer.Broadcast("/checker", new CheckerRequest(CheckerOpcode.Done));

                Finished = true;
            });
        }
    }
}
