using System.Net;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public static class Server
    {
        public static void Start()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost/");
            listener.Start();
            while (true)
            {
                var context = listener.GetContext();
                
                Task.Run(() => CommandHandler.Handle(context));
            }
        }
    }
}
