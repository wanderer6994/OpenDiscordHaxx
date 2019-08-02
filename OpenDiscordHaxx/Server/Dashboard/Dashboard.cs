using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class Dashboard : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            var req = new DashboardRequest<StatusUpdate>(DashboardOpcode.StatusUpdate);
            req.Data.Status = Server.ServerStatus;
            Send(JsonConvert.SerializeObject(req));
        }
    }
}
