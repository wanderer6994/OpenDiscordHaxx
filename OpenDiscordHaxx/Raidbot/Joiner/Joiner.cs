using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Joiner : RaidBot
    {
#pragma warning disable IDE1006
        private Invite _invite { get; set; }
#pragma warning restore IDE1006


        public Joiner(string invite, int threads)
        {
            Attack = new Attack(this) { Type = "Joiner", Bots = Server.Bots.Count };

            Threads = threads;
            try
            {
                _invite = new DiscordClient().GetInvite(invite.Split('/').Last());
            }
            catch (DiscordHttpException e)
            {
                if (e.Code == DiscordError.InvalidInvite || e.Code == DiscordError.UnknownInvite)
                {
                    Console.WriteLine($"[FATAL] {invite} is invalid");
                    
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
            int offset = TryMakeInvite();

            Parallel.ForEach(new List<RaidBotClient>(Server.Bots).Skip(offset + 1), new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                TryJoin(bot);
            });

            Server.OngoingAttacks.Remove(Attack);
        }


        public int TryMakeInvite()
        {
            int offset = 0;

            if (_invite.Type == InviteType.Guild)
            {
                for (int i = 0; i < Server.Bots.Count; i++)
                {
                    if (TryJoin(Server.Bots[i]))
                    {
                        try
                        {
                            GuildInvite inv = Server.Bots[i].Client.GetGuildInvite(_invite.Code);

                            _invite = Server.Bots[i].Client.CreateInvite(inv.Channel.Id);

                            offset = i;

                            break;
                        }
                        catch (DiscordHttpException ex)
                        {
                            if (ex.Code == DiscordError.MissingPermissions)
                            {
                                offset = i;

                                break;
                            }
                        }
                        catch { }
                    }
                }
            }

            return offset;
        }


        private bool TryJoin(RaidBotClient bot)
        {
            try
            {
                if (_invite.Type == InviteType.Guild)
                    bot.Client.JoinGuild(_invite.Code);
                else
                    bot.Client.JoinGroup(_invite.Code);

                return true;
            }
            catch (DiscordHttpException e)
            {
                switch (e.Code)
                {
                    case DiscordError.UnknownInvite:
                        Console.WriteLine($"[ERROR] unknown invite");

                        if (_invite.Type == InviteType.Group)
                            ShouldStop = true;
                        break;
                    case DiscordError.InvalidInvite:
                        Console.WriteLine($"[ERROR] invalid invite");
                        break;
                    default:
                        CheckError(e);
                        break;
                }
            }
            catch (RateLimitException) { }

            return false;
        }
    }
}
