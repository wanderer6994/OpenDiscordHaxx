using Discord;
using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Farmer
    {
        private StartFarmerRequest _request;
        private DiscordSocketClient _client;
        private Joiner _joiner;
        private GuildChannel _channel;

        public Farmer(StartFarmerRequest req)
        {
            _request = req;
        }


        public bool TryGetReceiver()
        {
            try
            {
                _client = new DiscordSocketClient();
                _client.Login(_request.ReceiverToken);
                while (!_client.LoggedIn) { Thread.Sleep(1); }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryCreateJoiner()
        {
            try
            {
                _joiner = new Joiner(_request.Invite, _request.Threads, false);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool TryFindChannel()
        {
            GuildInvite invite = new DiscordClient().GetGuildInvite(_request.Invite.Split('/').Last());

            try
            {
                foreach (var chnl in _client.GetGuildChannels(invite.Guild.Id))
                {
                    if (chnl.Type == ChannelType.Text)
                    {
                        _channel = chnl;
                        break;
                    }
                }

                if (_channel == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


        public void Farm()
        {
            _joiner.Start();

            foreach (var bot in Server.Bots)
            {
                try
                {
                    bot.Client.SendMessage(_channel.Id, "pls weekly");
                    bot.Client.SendMessage(_channel.Id, "pls daily");
                    bot.Client.SendMessage(_channel.Id, "pls beg");
                }
                catch { }
            }

            Thread.Sleep(200); //just for good measure

            //issue: we don't know how much we can give to receiver
            foreach (var bot in Server.Bots)
            {
                try
                {
                    bot.Client.SendMessage(_channel.Id, $"pls give <@{_client.User.Id}> 200");
                }
                catch { }
            }
        }
    }
}
