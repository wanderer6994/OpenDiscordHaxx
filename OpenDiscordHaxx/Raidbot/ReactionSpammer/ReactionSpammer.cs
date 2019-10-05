using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!BotStorage.DefaultEmojis.TryGetValue(_reaction, out _reaction))
            {
                _reaction = request.Reaction; //for some reason TryGetValue sets _reaction to null if not found

                if (_reaction.StartsWith(":"))
                {
                    ulong guildId = 0;

                    foreach (var channel in BotStorage.GuildChannels)
                    {
                        if (channel == _channelId)
                        {
                            guildId = channel.GuildId;

                            break;
                        }
                    }

                    List<Emoji> guildEmojis = BotStorage.CustomEmojis.Where(e => e.GuildId == guildId).ToList();

                    foreach (var emoji in guildEmojis)
                    {
                        if (emoji.Name == _reaction.Replace(":", ""))
                        {
                            _reaction = $"{emoji.Name}:{emoji.Id}";

                            break;
                        }
                    }
                }
            }
        }


        public override void Start()
        {
            Parallel.ForEach(new List<RaidBotClient>(Server.Bots), GetParallelOptions(), bot => 
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown exception occured: {ex}");
                }
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
