using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DiscordHaxx
{
    public class FarmerEndpoint : WebSocketBehavior
    {
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(e.Data);

            switch ((FarmerOpcode)obj.GetValue("op").ToObject(typeof(FarmerOpcode)))
            {
                case FarmerOpcode.Start:
                    StartFarmerRequest start = (StartFarmerRequest)obj.ToObject(typeof(StartFarmerRequest));

                    Farmer farmer = new Farmer(start);

                    if (!farmer.TryGetReceiver())
                    {
                        Send(JsonConvert.SerializeObject(new FarmerErrorRequest("Unable to log into client")));

                        return;
                    }

                    if (!farmer.TryCreateJoiner())
                    {
                        Send(JsonConvert.SerializeObject(new FarmerErrorRequest("Unable to get server invite")));

                        return;
                    }

                    if (!farmer.TryFindChannel())
                    {
                        Send(JsonConvert.SerializeObject(new FarmerErrorRequest("Unable to find a text channel")));

                        return;
                    }

                    Send(JsonConvert.SerializeObject(new FarmerRequest(FarmerOpcode.Start)));

                    farmer.Farm();

                    Send(JsonConvert.SerializeObject(new FarmerRequest(FarmerOpcode.Stopped)));
                    break;
            }
        }
    }
}
