namespace MathRace.Model
{
    using System;

    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class HallOfFamePlayerScore
    {
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return this.Timestamp.ToString("dd/MM/yyy hh:mm"); }
        }

        [JsonProperty(PropertyName = "player")]
        public string Player { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }
}