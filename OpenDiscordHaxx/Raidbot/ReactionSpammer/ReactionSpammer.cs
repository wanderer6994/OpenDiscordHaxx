using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class ReactionSpammer : RaidBot
    {
        private readonly ulong _channelId;
        private readonly ulong _messageId;
        private readonly string _reaction;
        private readonly bool _add;


        public ReactionSpammer(ReactionsRequest request)
        {
            Attack = new Attack(this) { Type = "Reaction spammer", Bots = Server.Bots.Count };

            Threads = request.Threads;
            _channelId = request.ChannelId;
            _messageId = request.MessageId;
            _reaction = request.Reaction;
            _add = request.Add;

            if (_channelId <= 0)
                throw new CheckException("Invalid channel ID");
            if (_messageId <= 0)
                throw new CheckException("Invalid message ID");
            if (string.IsNullOrWhiteSpace(_reaction))
                throw new CheckException("Invalid emoji");

            var emojis = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Emojis.json"));


            emojis.TryGetValue(_reaction, out _reaction);
        }


        public override void Start()
        {
            Parallel.ForEach(new List<RaidBotClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot => 
            {
                if (ShouldStop)
                    return;

                try
                {
                    if (_add)
                        bot.Client.AddMessageReaction(_channelId, _messageId, _reaction);
                    else
                        bot.Client.RemoveMessageReaction(_channelId, _messageId, _reaction);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.UnknownMessage:
                            Console.WriteLine($"[ERROR] Unknown message");
                            break;
                        case DiscordError.UnknownEmoji:
                            Console.WriteLine($"[ERROR] Unknown emoji");
                            break;
                        default:
                            CheckError(e);
                            break;
                    }
                }
                catch (RateLimitException) { }
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
