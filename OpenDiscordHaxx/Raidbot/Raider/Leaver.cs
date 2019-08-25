using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Leaver : RaidBot
    {
        private readonly ulong _guildId;
        private readonly bool _group;

        public Leaver(LeaveRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Leave, Bots = Server.Bots.Count };

            Threads = request.Threads;
            _guildId = request.GuildId;
            _group = request.Group;

            if (_guildId <= 0)
                throw new CheckException("Invalid guild ID");
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot =>
            {
                try
                {
                    if (ShouldStop)
                        return;

                    if (_group)
                        bot.LeaveGroup(_guildId);
                    else
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
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
