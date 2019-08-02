using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Flooder : IBot
    {
        private readonly FloodRequest _request;

        public Flooder(FloodRequest request)
        {
            _request = request;
        }


        public void Start()
        {
            while (true)
            {
                List<DiscordClient> validBots = Server.Bots;
                List<DiscordClient> nextBots = validBots;

                Parallel.ForEach(validBots, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
                {
                    while (true)
                    {
                        try
                        {
                            bot.SendMessage(_request.ChannelId, _request.Message, _request.Tts);

                            break;
                        }
                        catch (DiscordHttpException e)
                        {
                            switch (e.Code)
                            {
                                case DiscordError.AccountUnverified:
                                    Console.WriteLine($"[ERROR] {bot.User} is unverified");
                                    break;
                                case DiscordError.ChannelVerificationTooHigh:
                                    Console.WriteLine("[ERROR] channel verification too high");
                                    break;
                                case DiscordError.CannotSendEmptyMessage:
                                    Console.WriteLine("[ERROR] cannot send empty messages");
                                    break;
                                case DiscordError.UnknownChannel:
                                    Console.WriteLine("[ERROR] unknown channel");
                                    break;
                                default:
                                    Console.WriteLine($"[ERROR] Unknown: {e.Code} | {e.ErrorMessage}");
                                    break;
                            }

                            nextBots.Remove(bot);
                            break;
                        }
                        catch (RateLimitException e)
                        {
                            Thread.Sleep((int)e.RetryAfter);
                        }
                    }
                });

                validBots = nextBots;
            }
        }
    }
}
