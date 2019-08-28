using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Reactions : RaidBot
    {
        private readonly ReactionsRequest _request;


        public Reactions(ReactionsRequest request)
        {
            Attack = new Attack(this) { Type = "Reactions", Bots = Server.Bots.Count };

            Threads = request.Threads;
            _request = request;

            if (_request.ChannelId <= 0)
                throw new CheckException("Invalid channel ID");
            if (_request.MessageId <= 0)
                throw new CheckException("Invalid message ID");
            if (string.IsNullOrWhiteSpace(_request.Reaction))
                throw new CheckException("Invalid reaction");
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                try
                {
                    if (_request.Add)
                        bot.AddMessageReaction(_request.ChannelId, _request.MessageId, _request.Reaction);
                    else
                        bot.RemoveMessageReaction(_request.ChannelId, _request.MessageId, _request.Reaction);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.AccountUnverified:
                            Console.WriteLine($"[ERROR] {bot.User} is unverified");
                            break;
                        case DiscordError.UnknownChannel:
                            Console.WriteLine($"[ERROR] Unknown channel");
                            break;
                        case DiscordError.UnknownMessage:
                            Console.WriteLine($"[ERROR] Unknown message");
                            break;
                        case DiscordError.UnknownEmoji:
                            Console.WriteLine($"[ERROR] Unknown emoji");
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
