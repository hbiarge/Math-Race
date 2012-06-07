﻿using Newtonsoft.Json;

namespace MathRace.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerScore
    {
        [JsonProperty(PropertyName = "player")]
        public string Player { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }
}