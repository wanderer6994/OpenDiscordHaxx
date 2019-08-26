using System;
using Discord;

namespace DiscordHaxx
{
    public class FloodClient
    {
        private readonly DiscordClient _client;
        private readonly ulong _channelId;


        public FloodClient(DiscordClient client, ulong id, bool dm)
        {
            _client = client;
            _channelId = dm ? _client.CreateDM(id).Id : id;
        }


        public bool TrySendMessage(string message, Embed embed)
        {
            try
            {
                _client.SendMessage(_channelId, message, false, embed);
            }
            catch (DiscordHttpException e)
            {
                switch (e.Code)
                {
                    case DiscordError.AccountUnverified:
                        Console.WriteLine($"[ERROR] {_client.User} is unverified");
                        break;
                    case DiscordError.ChannelVerificationTooHigh:
                        Console.WriteLine("[ERROR] channel verification too high");
                        break;
                    case DiscordError.UnknownChannel:
                        Console.WriteLine("[ERROR] unknown channel");
                        break;
                    default:
                        Console.WriteLine($"[ERROR] Unknown: {e.Code} | {e.ErrorMessage}");
                        break;
                }

                return false;
            }
            catch (RateLimitException) { }


            return true;
        }
    }
}
