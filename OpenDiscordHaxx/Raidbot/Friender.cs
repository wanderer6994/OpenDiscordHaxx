using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Friender : IBot
    {
        private string _username;
        private uint _discriminator;


        public Friender(FriendRequest request)
        {
            string[] credentials = request.User.Split('#');

            if (credentials.Length < 2 || !uint.TryParse(credentials[1], out _discriminator))
            {
                Console.WriteLine($"[FATAL] Invalid user format");

                throw new InvalidOperationException();
            }

            _username = credentials[0];
        }


        public void Start()
        {
            Parallel.ForEach(Program.Bots, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                while (true)
                {
                    try
                    {
                        bot.SendFriendRequest(_username, _discriminator);

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
