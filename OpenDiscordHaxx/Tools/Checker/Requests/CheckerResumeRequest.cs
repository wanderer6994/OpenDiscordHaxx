using Newtonsoft.Json;

namespace DiscordHaxx
{
    class CheckerResumeRequest : CheckerRequest
    {
        [JsonProperty("progress")]
        public CheckerProgress Progress { get; set; }


        public CheckerResumeRequest() : base(CheckerOpcode.Resume)
        { }
    }
}
