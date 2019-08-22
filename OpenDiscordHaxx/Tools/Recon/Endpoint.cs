using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class ReconEndpoint : WebSocketBehavior
    {
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
                            return;

                        ServerRecon recon = new ServerRecon
                        {
                            Name = guild.Name,
                            Description = guild.Description == null ? "No description" : guild.Description,
                            Region = guild.Region,
                            VerificationLevel = guild.VerificationLevel.ToString(),
                            VanityInvite = guild.VanityInvite == null ? "None" : guild.VanityInvite,
                            BotsInGuild = bots == 0 ? "None" : bots.ToString()
                        };

                        foreach (var role in guild.Roles.Where(r => r.Mentionable))
                            recon.Roles.Add(new RoleInfo(role));

                        Send(JsonConvert.SerializeObject(recon));
                    });
                    break;
            }
        }
    }
}
