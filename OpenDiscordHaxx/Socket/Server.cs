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
            _server.AddWebSocketService<Dashboard>("/dashboard");
            _server.AddWebSocketService<Bot>("/bot");
            _server.AddWebSocketService<RaidBot>("/bot/raid");
            _server.AddWebSocketService<Checker>("/bot/checker");
            _server.Start();


            while (!_server.IsListening)
                Thread.Sleep(100);
        }

        public static void Broadcast<T>(string endpoint, DashboardRequest<T> request) where T : new()
        {
            if (_server.IsListening)
            {
                _server.WebSocketServices[endpoint].Sessions
                                .Broadcast(JsonConvert.SerializeObject(request));
            }
        }
    }
}
