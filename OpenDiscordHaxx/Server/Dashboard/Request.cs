using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class DashboardRequest<T> where T : new()
    {
        [JsonProperty("opcode")]
        public DashboardOpcode Opcode { get; private set; }


        [JsonProperty("data")]
        public T Data { get; private set; }


        public DashboardRequest(DashboardOpcode op)
        {
            Opcode = op;
            Data = new T();
        }
    }
}
