using Discord.Gateway;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class VCSpammer : RaidBot
    {
        private VCRequest _request;
        private List<DiscordSocketClient> _clients;

        public VCSpammer(VCRequest request)
        {
            _request = request;
            Threads = request.Threads;

            if (_request.GuildId <= 0)
                throw new CheckException("Invalid server ID");
            if (_request.ChannelId <= 0 && _request.Join)
                throw new CheckException("Invalid channel ID");

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
            Parallel.ForEach(_clients, new ParallelOptions() { MaxDegreeOfParallelism = Threads }, bot =>
            {
                if (ShouldStop)
                    return;

                if (_request.Join)
                    bot.JoinVoiceChannel(_request.GuildId, _request.ChannelId);
                else
                    bot.LeaveVoiceChannel(_request.GuildId);
            });

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
