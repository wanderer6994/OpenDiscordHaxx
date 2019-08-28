using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class StatusUpdate : DashboardInnerRequest
    {
        public StatusUpdate() : base(DashboardOpcode.StatusUpdate)
        { }


        [JsonProperty("status")]
#pragma warning disable IDE0052
        private readonly string _status = Server.ServerStatus;
#pragma warning restore IDE0052
    }
}
