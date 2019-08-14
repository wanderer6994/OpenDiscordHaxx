using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : Bot
    {
        private readonly FriendRequest _recipient;


        public Friender(FriendRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Friend, Bots = Server.Bots.Count };

            _recipient = request;
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                if (ShouldStop)
                    return;

                try
                {
                    bot.SendFriendRequest(_recipient.Username, _recipient.Discriminator);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.InvalidRecipient:
                            Console.WriteLine($"[ERROR] invalid recipient");
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
