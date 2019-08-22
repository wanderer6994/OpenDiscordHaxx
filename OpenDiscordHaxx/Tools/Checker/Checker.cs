using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Checker
    {
        public int Total { get; private set; }
        public int Valid { get; private set; }
        public int Invalid { get; private set; }

        public bool Finished { get; private set; }


        public async void StartAsync()
        {
            await Task.Run(() =>
            {
                if (Server.Bots.Count == 0)
                {
                    SocketServer.Broadcast("/bot/checker", new CheckerErrorRequest("notokens"));
                    Finished = true;
                    return;
                }

                SocketServer.Broadcast("/bot/checker", new CheckerStartedRequest());

                Total = Server.Bots.Count;
                foreach (var client in new List<DiscordClient>(Server.Bots))
                {
                    BotCheckedRequest req = new BotCheckedRequest()
                    { Bot = BotInfo.FromClient(client) };
                    req.Progress.Total = Total;

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
                        SocketServer.Broadcast("/bot/checker", new CheckerErrorRequest("ratelimit"));
                        Finished = true;
                        break;
                    }


                    if (req.Valid)
                        Valid++;
                    else
                    {
                        Server.Bots.Remove(client);
                        Invalid++;
                    }

                    req.Progress.Valid = Valid;
                    req.Progress.Invalid = Invalid;


                    SocketServer.Broadcast("/bot/checker", req);
                }

                if (Total > Server.Bots.Count)
                {
                    StringBuilder tokensToAdd = new StringBuilder();
                    foreach (var bot in Server.Bots)
                        tokensToAdd.AppendLine(bot.Token);

                    File.WriteAllText("Tokens-checked.txt", tokensToAdd.ToString());
                }

                SocketServer.Broadcast("/bot/checker", new CheckerRequest(CheckerOpcode.Done));

                Finished = true;
            });
        }
    }
}
