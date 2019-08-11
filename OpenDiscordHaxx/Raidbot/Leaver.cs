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
            Attack = new Attack() { Type = RaidOpcode.Leave, Bots = Server.Bots.Count };


            _guildId = request.GuildId;
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                while (true)
                {
                    try
                    {
                        bot.LeaveGuild(_guildId);

                        break;
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

                        break;
                    }
                    catch (RateLimitException e)
                    {
                        Thread.Sleep((int)e.RetryAfter);
                    }
                }
            });


            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
