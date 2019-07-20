using System;

namespace DiscordHaxx
{
    class CheckException : Exception
    {
        public string Error { get; private set; }

        public CheckException(string error)
        {
            Error = error;
        }
    }
}
