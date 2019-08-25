using Discord;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class ReconEndpoint : WebSocketBehavior
    {
        private static int _nextId;



        private int _id;


        protected override void OnOpen()
        {
            _id = _nextId;
            _nextId++;

            Send(JsonConvert.SerializeObject(new ReconRequest(_id, ReconOpcode.Id)));
        }


        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            switch (JsonConvert.DeserializeObject<ReconRequest>(e.Data).Opcode)
            {
                case ReconOpcode.StartRecon:
                    Task.Run(() =>
                    {
                        var req = JsonConvert.DeserializeObject<StartReconRequest>(e.Data);

                        int bots = 0;
                        Guild guild = null;


                        foreach (var bot in Server.Bots)
                        {
                            try
                            {
                                guild = bot.GetGuild(req.GuildId);

                                bots++;
                            }
                            catch { }
                        }

                        if (guild == null)
                        {
                            SocketServer.Broadcast("/recon", new ReconRequest(_id, ReconOpcode.ReconFailed));

                            return;
                        }

                        ServerRecon recon = new ServerRecon(_id)
                        {
                            Name = guild.Name,
                            Description = guild.Description ?? "No description",
                            Region = guild.Region,
                            VerificationLevel = guild.VerificationLevel.ToString(),
                            VanityInvite = guild.VanityInvite ?? "None",
                            BotsInGuild = $"{bots.ToString()}/{Server.Bots.Count}"
                        };

                        foreach (var role in guild.Roles.Where(r => r.Mentionable))
                            recon.Roles.Add(new RoleInfo(role));

                        SocketServer.Broadcast("/recon", recon);
                    });
                    break;
            }
        }
    }
}
