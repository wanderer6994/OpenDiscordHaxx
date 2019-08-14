using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Leaver : Bot
    {
        private readonly ulong _guildId;

        public Leaver(LeaveRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Leave, Bots = Server.Bots.Count };


            _guildId = request.GuildId;
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                try
                {
                    if (ShouldStop)
                        return;

                    bot.LeaveGuild(_guildId);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.AccountUnverified:
                            Console.WriteLine($"[ERROR] {bot.User} is unverified");
                            break;
                        case DiscordError.UnknownGuild:
                            Console.WriteLine("[ERROR] invalid guild");
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
