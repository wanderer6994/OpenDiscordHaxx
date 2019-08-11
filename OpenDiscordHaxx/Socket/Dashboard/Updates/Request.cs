using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class DashboardRequest<T> where T : new()
    {
        [JsonProperty("opcode")]
        public DashboardOpcode Opcode { get; private set; }


        [JsonProperty("data")]
        public T Data { get; set; }


        public DashboardRequest(DashboardOpcode op)
        {
            Opcode = op;
            Data = new T();
        }


        public static implicit operator string(DashboardRequest<T> instance)
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}
