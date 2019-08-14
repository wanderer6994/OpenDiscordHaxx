using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Joiner : Bot
    {
#pragma warning disable IDE1006
        private Invite _invite { get; set; }
#pragma warning restore IDE1006


        public Joiner(JoinRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Join, Bots = Server.Bots.Count };


            try
            {
                _invite = new DiscordClient().GetGuildInvite(request.Invite.Split('/').Last());
            }
            catch (DiscordHttpException e)
            {
                if (e.Code == DiscordError.InvalidInvite || e.Code == DiscordError.UnknownInvite)
                {
                    Console.WriteLine($"[FATAL] {request.Invite} is invalid");
                    
                    throw new CheckException("Invalid invite");
                }
                else
                {
                    Console.WriteLine($"[ERROR] Unknown: {e.Code} | {e.ErrorMessage}");

                    throw new CheckException($"Code: {e.Code}");
                }
            }
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                try
                {
                    if (ShouldStop)
                        return;

                    bot.JoinGuild(_invite.Code);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.UnknownInvite:
                            Console.WriteLine($"[ERROR] unknown invite");
                            break;
                        case DiscordError.InvalidInvite:
                            Console.WriteLine($"[ERROR] invalid invite");
                            break;
                        case DiscordError.AccountUnverified:
                            Console.WriteLine($"[ERROR] {bot.User} is unverified");
                            break;
                        default:
                            Console.WriteLine($"[ERROR] Unknown: {e.Code} | {e.ErrorMessage}");
                            break;
                    }
                }
                catch (RateLimitException) { }
                catch
                {

                }
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
