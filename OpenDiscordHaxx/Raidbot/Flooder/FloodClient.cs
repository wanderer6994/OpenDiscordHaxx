using System;
using Discord;

namespace DiscordHaxx
{
    public class FloodClient
    {
        private readonly DiscordClient _client;
        private readonly ulong _channelId;
        private readonly string _message;


        public FloodClient(DiscordClient client, ulong id, bool dm, string message)
        {
            _client = client;
            _channelId = dm ? _client.CreateDM(id).Id : id;
            _message = message;
        }


        public bool TrySendMessage(Embed embed)
        {
            
            try
            {
                _client.SendMessage(_channelId, _message, false, embed);
            }
            catch (DiscordHttpException e)
            {
                if (e.Code == DiscordError.ChannelVerificationTooHigh)
                    Console.WriteLine("[ERROR] channel verification too high");
                else
                    RaidBot.CheckError(e);

                return false;
            }
            catch (RateLimitException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown exception occured: {ex}");

                return false;
            }


            return true;
        }
    }
}
