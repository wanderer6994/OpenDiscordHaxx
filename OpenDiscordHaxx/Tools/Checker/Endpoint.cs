using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using Discord;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DiscordHaxx
{
    public class CheckerEndpoint : WebSocketBehavior
    {
        private static Checker _checker;


        protected override void OnOpen()
        {
            if (_checker == null || _checker.Finished)
            {
                _checker = new Checker();
                _checker.StartAsync();
            }
            else
                Send(JsonConvert.SerializeObject(new CheckerProgress() { Total = _checker.Total,
                                                                         Valid = _checker.Valid,
                                                                         Invalid = _checker.Invalid }));
        }
    }
}
