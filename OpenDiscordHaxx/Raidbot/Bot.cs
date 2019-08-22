namespace DiscordHaxx
{
    public abstract class Bot
    {
        public int Threads { get; protected set; }
        public bool ShouldStop { get; set; }
        public Attack Attack { get; protected set; }

        public abstract void Start();
    }
}
