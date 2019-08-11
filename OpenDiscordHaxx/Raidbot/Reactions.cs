using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Reactions : Bot
    {
        private Attack attack;

        public Attack GetAttack()
        {
            return attack;
        }

        private void SetAttack(Attack value)
        {
            attack = value;
        }

        private readonly ReactionsRequest _request;


        public Reactions(ReactionsRequest req)
        {
            SetAttack(new Attack() { Type = RaidOpcode.React, Bots = Server.Bots.Count });

            _request = req;
        }


        public override void Start()
        {
            Parallel.ForEach(new List<DiscordClient>(Server.Bots), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, bot =>
            {
                try
                {
                    if (_request.Add)
                        bot.AddMessageReaction(_request.ChannelId, _request.MessageId, _request.Reaction);
                    else
                        bot.RemoveMessageReaction(_request.ChannelId, _request.MessageId, _request.Reaction);
                }
                catch (DiscordHttpException e)
                {
                    switch (e.Code)
                    {
                        case DiscordError.UnknownChannel:
                            Console.WriteLine($"[ERROR] Unknown channel");
                            break;
                        case DiscordError.UnknownMessage:
                            Console.WriteLine($"[ERROR] Unknown message");
                            break;
                        case DiscordError.UnknownEmoji:
                            Console.WriteLine($"[ERROR] Unknown emoji");
                            break;
                        default:
                            Console.WriteLine($"[ERROR] Unknown: {e.Code} | {e.ErrorMessage}");
                            break;
                    }
                }
            });

            Server.OngoingAttacks.Remove(GetAttack());
        }
    }
}
