using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : RaidBot
    {
        private readonly FriendRequest _request;


        public Friender(FriendRequest request)
        {
            Attack = new Attack(this) { Type = "Friender", Bots = Server.Bots.Count };

            Threads = request.Threads;
            _request = request;

            if (string.IsNullOrWhiteSpace(_request.Username))
                throw new CheckException("Please enter a username");
            if (_request.Discriminator <= 1)
                throw new CheckException("Invalid discriminator");
        }


        public override void Start()
        {
            Parallel.ForEach(new List<RaidBotClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = _request.Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                try
                {
                    if (bot.SocketClient)
                    {
                        var results = bot.Relationships.Where(b => b.User.Username == _request.Username && b.User.Discriminator == _request.Discriminator).ToList();

                        if (results.Count > 0)
                        {
                            if (results[0].Type == (RelationshipType.Friends & RelationshipType.OutgoingRequest))
                                return;
                            else if (results[0].Type == (RelationshipType.Blocked & RelationshipType.IncomingRequest))
                                results[0].Remove();
                        }
                    }

                    bot.Client.SendFriendRequest(_request.Username, _request.Discriminator);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.InvalidRecipient:
                            Console.WriteLine($"[ERROR] invalid recipient");
                            break;
                        case DiscordError.AccountUnverified:
                            Console.WriteLine($"[ERROR] {bot.Client.User} is unverified");
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
