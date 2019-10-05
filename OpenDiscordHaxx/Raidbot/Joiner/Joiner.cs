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
        private Invite _invite;
        private readonly PartialGuild _partialGuild; //only active when invite is of type Guild
        private readonly GuildChannel _channel; //only active when invite is of type Guild
        private readonly bool _enableAntiTrack;
#pragma warning restore IDE1006


        public Joiner(string invite, int threads, bool enableAntiTrack)
        {
            Attack = new Attack(this) { Type = "Joiner", Bots = Server.Bots.Count };

            Threads = threads;
            try
            {
                string invCode = invite.Split('/').Last();
                DiscordClient tempClient = new DiscordClient();

                if (BotStorage.Invites.Where(i => i == invCode).Count() > 0)
                    _invite = BotStorage.Invites.First(i => i == invCode);
                else
                {
                    _invite = tempClient.GetInvite(invCode);

                    BotStorage.AddInvite(_invite);
                }

                if (_invite.Type == InviteType.Guild)
                {
                    GuildInvite gInvite = tempClient.GetGuildInvite(_invite.Code);

                    _partialGuild = gInvite.Guild;
                    _channel = gInvite.Channel;
                }
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

            _enableAntiTrack = enableAntiTrack;
        }


        public override void Start()
        {
            int offset = TryMakeInvite();

            Parallel.ForEach(new List<RaidBotClient>(Server.Bots).Skip(offset + 1), GetParallelOptions(), bot =>
            {
                if (ShouldStop)
                    return;

                TryJoin(bot);
            });

            Server.OngoingAttacks.Remove(Attack);
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
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown exception occured: {ex}");
            }

            return false;
        }


        public int TryMakeInvite()
        {
            int offset = 0;

            if (_enableAntiTrack && _invite.Type == InviteType.Guild)
            {

                Guild guild = null;

                for (int i = 0; i < Server.Bots.Count; i++)
                {
                    if (ShouldStop)
                        break;

                    if (TryJoin(Server.Bots[i]))
                    {
                        try
                        {
                            if (guild == null)
                            {
                                guild = Server.Bots[i].Client.GetGuild(_partialGuild.Id);
                                BotStorage.CustomEmojis.AddRange(guild.Emojis);
                                BotStorage.GuildChannels.AddRange(guild.GetChannels());
                            }

                            if (guild.VanityInvite != null)
                                _invite = Server.Bots[i].Client.GetInvite(guild.VanityInvite);
                            else
                                _invite = Server.Bots[i].Client.CreateInvite(_channel);

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
    }
}
