using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DiscordHaxx
{
    public class AttacksUpdate : DashboardInnerRequest
    {
        public AttacksUpdate() : base(DashboardOpcode.AttacksUpdate)
        { }

        [JsonProperty("attacks")]
#pragma warning disable IDE0052
        private readonly ObservableCollection<Attack> _attacks = Server.OngoingAttacks;
#pragma warning restore IDE0052
    }
}
