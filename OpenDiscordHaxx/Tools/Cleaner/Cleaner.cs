using Discord;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordHaxx
{
    public class Cleaner
    {
        public bool Finished { get; private set; }
        private readonly CleanOptions _options;

        public Cleaner(CleanOptions options)
        {
            _options = options;
        }


        //Issue: Currently try catch blocks here are quite lazy. might wanna add some stuff
        public async void StartAsync()
        {
            await Task.Run(() =>
            {
                foreach (var bot in Server.Bots)
                {
                    if (_options.RemoveGuilds)
                    {
                        foreach (var guild in bot.GetGuilds())
                        {
                            try
                            {
                                if (guild.Owner)
                                    guild.Delete();
                                else
                                    guild.Leave();

                                Thread.Sleep(100);
                            }
                            catch { }
                        }
                    }

                    if (_options.RemoveRelationships)
                    {
                        try
                        {
                            foreach (var relationship in bot.GetRelationships())
                            {
                                relationship.Remove();

                                Thread.Sleep(100);
                            }
                        }
                        catch { }
                    }

                    if (_options.RemoveDMs)
                    {
                        try
                        {
                            foreach (var dm in bot.GetPrivateChannels())
                            {
                                dm.Leave();

                                Thread.Sleep(100);
                            }
                        }
                        catch { }
                    }

                    if (_options.ResetProfile)
                    {
                        try
                        {
                            if (bot.User.EmailVerified)
                                bot.User.SetHypesquad(Hypesquad.None);

                            if (bot.User.AvatarId != null)
                                bot.User.Modify(new UserSettings() { Avatar = null });
                        }
                        catch { }

                        try
                        {
                            foreach (var connection in bot.GetProfile(bot.User.Id).ConnectedAccounts)
                            {
                                connection.Remove();

                                Thread.Sleep(100);
                            }
                        }
                        catch { }
                    }

                    SocketServer.Broadcast("/cleaner", new AccountCleanedRequest(bot));
                }

                Finished = true;
                SocketServer.Broadcast("/cleaner", new CleanerRequest(CleanerOpcode.CleanerFinished));
            });
        }
    }
}
