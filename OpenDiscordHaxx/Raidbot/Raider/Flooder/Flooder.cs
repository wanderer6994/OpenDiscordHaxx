using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace DiscordHaxx
{
    public class Flooder : RaidBot
    {
        private readonly FloodRequest _request;
        private List<FloodClient> _clients;
        private bool _ready;

        public Flooder(FloodRequest request)
        {
            Attack = new Attack(this) { Type = RaidOpcode.Flood, Bots = Server.Bots.Count };

            Threads = request.Threads;
            _request = request;

            if (string.IsNullOrWhiteSpace(_request.Message))
                throw new CheckException("Cannot send empty messages");

            if (_request.ChannelId <= 0)
                throw new CheckException("Invalid channel ID");

            _clients = new List<FloodClient>();

            if (request.DM)
            {
                Task.Run(() =>
                {
                    foreach (var bot in Server.Bots)
                    {
                        try
                        {
                            _clients.Add(new FloodClient(bot, request.ChannelId, true));
                        }
                        catch { }
                    }

                    _ready = true;
                });
            }
            else
            {
                foreach (var bot in Server.Bots)
                    _clients.Add(new FloodClient(bot, request.ChannelId, false));

                _ready = true;
            }
        }


        public override void Start()
        {
            List<FloodClient> nextClients = new List<FloodClient>(_clients);

            while (!_ready) { Thread.Sleep(200); }

            while (true)
            {
                if (ShouldStop)
                    break;

                Parallel.ForEach(_clients, new ParallelOptions() { MaxDegreeOfParallelism = _request.Threads }, bot =>
                {
                    if (ShouldStop)
                        return;

                    if (!bot.TrySendMessage(_request.Message, _request.UseEmbed ? _request.Embed : null))
                        nextClients.Remove(bot);
                });


                _clients = nextClients;

                if (_clients.Count == 0)
                    break;
            }

            Server.OngoingAttacks.Remove(Attack);
        }
    }
}
