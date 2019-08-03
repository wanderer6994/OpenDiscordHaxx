using Newtonsoft.Json;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class Dashboard : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Send(JsonConvert.SerializeObject(new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate)
                                                    { Data = new StatusUpdate() { Status = Server.ServerStatus } }));
            Send(JsonConvert.SerializeObject(new DashboardRequest<OverlookUpdate>(DashboardOpcode.OverlookUpdate)
                                                    { Data = new OverlookUpdate() { Accounts = Server.Bots.Count, Attacks = Server.OngoingAttacks } }));
        }
    }
}
