using System;
using System.Drawing;
using Discord;
using Newtonsoft.Json;

namespace DiscordHaxx
{
    public class ModRequest
    {
        [JsonProperty("id")]
#pragma warning disable CS0649
        private readonly string _id;
#pragma warning restore CS0649

        public ulong Id
        {
            get { return ulong.Parse(_id); }
        }


        [JsonProperty("hypesquad")]
#pragma warning disable CS0649
        private readonly string _hypesquad;
#pragma warning restore CS0649

        public Hypesquad Hypesquad
        {
            get { return (Hypesquad)Enum.Parse(typeof(Hypesquad), _hypesquad); }
        }


        [JsonProperty("status")]
        public string Status { get; private set; }


        [JsonProperty("avatar")]
        private string _avatar;

        public Image Avatar
        {
            get
            {
                return new ReverseDiscordImage() { Base64 = _avatar }.Image;
            }
        }


        [JsonProperty("set_all")]
        public bool SetAll { get; private set; }
    }
}
