using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : Bot
    {
        private readonly FriendRequest _request;


        public Friender(FriendRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Friend, Bots = Server.Bots.Count };

            Threads = request.Threads;
            _request = request;

            if (string.IsNullOrWhiteSpace(_request.Username))
                throw new CheckException("Please enter a username");
            if (_request.Discriminator <= 1)
                throw new CheckException("Invalid discriminator");
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = _request.Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                try
                {
                    bot.SendFriendRequest(_request.Username, _request.Discriminator);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.InvalidRecipient:
                            Console.WriteLine($"[ERROR] invalid recipient");
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
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
