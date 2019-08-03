using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    class BotSuccessStatus
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }
    }


    public class Bot : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            BotSuccessStatus status = new BotSuccessStatus();

            try
            {
                CommandHandler.TryHandle(e.Data);

                status.Succeeded = true;
            }
            catch (CheckException ex)
            {
                status.Succeeded = false;
                status.Message = ex.Error;
            }

            Send(JsonConvert.SerializeObject(status));
        }
    }
}
