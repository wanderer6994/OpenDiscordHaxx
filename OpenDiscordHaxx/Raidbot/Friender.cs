using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : IBot
    {
        private readonly FriendRequest _recipient;


        public Friender(FriendRequest request)
        {
            _recipient = request;
        }


        public void Start()
        {
            Parallel.ForEach(Server.Bots, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                while (true)
                {
                    try
                    {
                        bot.SendFriendRequest(_recipient.Username, _recipient.Discriminator);

                        break;
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

                        break;
                    }
                }
            });
        }
    }
}
