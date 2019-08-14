using Newtonsoft.Json;
using System.Threading;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public static class SocketServer
    {
        private static WebSocketServer _server;

        public static void Start()
        {
            _server = new WebSocketServer("ws://localhost");
            _server.AddWebSocketService<DashboardEndpoint>("/dashboard");
            _server.AddWebSocketService<BotEndpoint>("/bot");
            _server.AddWebSocketService<RaidBotEndpoint>("/bot/raid");
            _server.AddWebSocketService<CheckerEndpoint>("/bot/checker");
            _server.Start();
        }

        public static void Broadcast<T>(DashboardOpcode op, T requestData) where T : new()
        {
            DashboardRequest<T> req = new DashboardRequest<T>(op);
            req.Data = requestData;

            if (_server.IsListening)
            {
                _server.WebSocketServices["/dashboard"].Sessions
                                .Broadcast(JsonConvert.SerializeObject(req));
            }
        }
    }
}
