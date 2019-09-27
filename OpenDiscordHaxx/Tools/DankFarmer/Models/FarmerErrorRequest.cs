using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class FarmerErrorRequest : FarmerRequest
    {
        public FarmerErrorRequest(string errorMessage) : base(FarmerOpcode.Error)
        {
            ErrorMessage = errorMessage;
        }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; private set; }
    }
}
