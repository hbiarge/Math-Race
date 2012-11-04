namespace MathRace.Model
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class RaceHistory
    {
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return this.Timestamp.ToString("dd/MM/yyy hh:mm"); }
        }

        [JsonProperty(PropertyName = "scores")]
        public List<PlayerScore> Scores { get; set; }
    }
}