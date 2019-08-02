using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public static class Server
    {
        public static List<DiscordClient> Bots = new List<DiscordClient>();


        private static string _serverStatus;
        public static string ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;

                if (SocketServer.Running)
                {
                    var req = new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate);
                    req.Data.Status = _serverStatus;
                    SocketServer.Send(req);
                }
            }
        }


        public static void LoadAccounts()
        {
            foreach (var token in File.ReadAllLines("Tokens.txt"))
            {
                try
                {
                    Bots.Add(new DiscordClient(token));
                }
                catch //lazy 
                {
                }
            }
        }
    }
}
