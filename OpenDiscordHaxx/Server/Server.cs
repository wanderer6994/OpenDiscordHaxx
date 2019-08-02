using Newtonsoft.Json;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public static class SocketServer
    {
        private static WebSocketServer _server;
        public static bool Running { get; private set; }

        public static void Start()
        {
            _server = new WebSocketServer("ws://localhost");
            _server.AddWebSocketService<Dashboard>("/dashboard");
            _server.AddWebSocketService<Bot>("/bot");
            _server.Start();
            Running = true;
        }

        public static void Send<T>(DashboardRequest<T> request) where T : new()
        {
            _server.WebSocketServices["/dashboard"].Sessions.Broadcast(JsonConvert.SerializeObject(request));
        }
    }
}
