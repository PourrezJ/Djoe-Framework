using Newtonsoft.Json;
using Server.Utils.Enums;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class WorldData
    {
        public DateTime WorldTime = new DateTime(1900, 3, 1, 12, 0, 0);

        [JsonIgnore]
        public List<BanData> BanList = new List<BanData>();


        public WeatherType WeatherActual = WeatherType.Sunny;
    }
}
