namespace DiscordHaxx
{
    public abstract class Bot
    {
        public Attack Attack { get; protected set; }

        public abstract void Start();
    }
}
