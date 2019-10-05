using Discord.Gateway;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Discord;

namespace DiscordHaxx
{
    public class VCSpammer : RaidBot
    {
        private readonly VCRequest  _request;
        private readonly List<DiscordSocketClient> _clients;

        public VCSpammer(VCRequest request)
        {
            _request = request;
            Threads = 1;

            if (_request.GuildId <= 0)
                throw new CheckException("Invalid server ID");
            if (_request.ChannelId <= 0 && _request.Join)
                throw new CheckException("Invalid channel ID");

            if (BotStorage.GuildChannels.Where(c => c.Id == _request.ChannelId).Count() > 0)
            {
                if (BotStorage.GuildChannels.First(c => c.Id == _request.ChannelId).Type != ChannelType.Voice)
                    throw new CheckException("Channel is not a voice channel");
            }

            _clients = new List<DiscordSocketClient>();
            foreach (var bot in new List<RaidBotClient>(Server.Bots))
            {
                if (bot.SocketClient)
                    _clients.Add((DiscordSocketClient)bot.Client);
            }

            Attack = new Attack(this) { Type = "VC Spammer", Bots = _clients.Count };
        }

        public override void Start()
        {
            foreach (var bot in _clients)
            {
                if (ShouldStop)
                    break;

                if (_request.Join)
                    bot.JoinVoiceChannel(_request.GuildId, _request.ChannelId);
                else
                    bot.LeaveVoiceChannel(_request.GuildId);

                Thread.Sleep(_request.Delay);
            }

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
