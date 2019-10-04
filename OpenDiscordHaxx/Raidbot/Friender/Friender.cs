using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : RaidBot
    {
        private readonly ulong _userId;


        public Friender(FriendRequest request)
        {
            Attack = new Attack(this) { Type = "Friender", Bots = Server.Bots.Count };

            Threads = request.Threads;
            _userId = request.UserId;

            if (request.UserId <= 0)
                throw new CheckException("Invalid user ID");
            else
            {
                if (BotStorage.Users.Where(u => u.Id == _userId).Count() == 0)
                {
                    User foundUser = null;

                    foreach (var bot in new List<RaidBotClient>(Server.Bots))
                    {
                        try
                        {
                            foundUser = bot.Client.GetUser(_userId);

                            break;
                        }
                        catch { }
                    }

                    if (foundUser == null)
                        throw new CheckException("User does not exist");
                }
            }
        }


        public override void Start()
        {
            Parallel.ForEach(new List<RaidBotClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                try
                {
                    if (bot.SocketClient)
                    {
                        var results = bot.Relationships.Where(b => b.User.Id == _userId).ToList();

                        if (results.Count > 0)
                        {
                            if (results[0].Type == RelationshipType.Friends)
                                return;
                            else if (results[0].Type == RelationshipType.Blocked || results[0].Type == RelationshipType.IncomingRequest)
                                results[0].Remove();
                        }
                    }

                    bot.Client.SendFriendRequest(_userId);
                }
                catch (DiscordHttpException e)
                {
                    if (e.Code == DiscordError.InvalidRecipient)
                        Console.WriteLine($"[ERROR] invalid recipient");
                    else
                        CheckError(e);
                }
                catch (RateLimitException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown exception occured: {ex}");
                }
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
