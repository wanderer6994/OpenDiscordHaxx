using System.Collections.Generic;
using System.Linq;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class DashboardEndpoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Send(new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate)
                                     { Data = new StatusUpdate() { Status = Server.ServerStatus } });
            Send(new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate)
                                     { Data = new OverlookUpdate() { Accounts = Server.Bots.Count, Attacks = Server.OngoingAttacks.Count } });
            Send(new DashboardRequest<List<Attack>>(DashboardOpcode.AttacksUpdate)
                                     { Data = Server.OngoingAttacks.ToList() });
        }
    }
}
