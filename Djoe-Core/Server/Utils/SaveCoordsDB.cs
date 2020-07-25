using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Scripts;

namespace Server.Utils
{
    /*
    public class SaveCoordsDB : BaseScript
    {
        public static Dictionary<Player, Tuple<Vector3, float>> LastCoordsInCache = new Dictionary<Player, Tuple<Vector3, float>>();
        public SaveCoordsDB()
        {
            EventHandlers["djoe:saveLastCoords"] += new Action<Player, Vector3, float>(SaveLastCoords);
            EventHandlers["djoe:ImDead"] += new Action<Player, bool>(OnPlayerDead);

            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }

        private void OnPlayerDead([FromSource]Player player, bool isDead)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);

            if (LoadCharacter.Characters.ContainsKey(sid))
            {
                LoadCharacter.Characters[sid].SetDead(isDead);
            }
        }

        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);

            LoadCharacter.Characters.Remove(sid);

            try
            {
                Vector3 lastCoords = LastCoordsInCache[player].Item1;
                float lastHeading = LastCoordsInCache[player].Item2;

                UPlayerCoords UPC = new UPlayerCoords()
                {
                    x = lastCoords.X,
                    y = lastCoords.Y,
                    z = lastCoords.Z,
                    heading = lastHeading
                };

                string pos = JsonConvert.SerializeObject(UPC);

                Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });

                LastCoordsInCache.Remove(player);
            }
            catch
            {

            }
        }

        private void SaveLastCoords([FromSource] Player source, Vector3 lastCoords, float lastHeading)
        {
            LastCoordsInCache[source] = new Tuple<Vector3, float>(lastCoords, lastHeading);
        }

        [Tick]
        private async Task SaveLastCoordsTick()
        {
            await Delay(300000);
            foreach (var source in LastCoordsInCache)
            {
                string sid = ("steam:" + source.Key.Identifiers["steam"]);
                try
                {
                    Vector3 lastCoords = source.Value.Item1;
                    float lastHeading = source.Value.Item2;

                    UPlayerCoords UPC = new UPlayerCoords()
                    {
                        x = lastCoords.X,
                        y = lastCoords.Y,
                        z = lastCoords.Z,
                        heading = lastHeading
                    };

                    string pos = JsonConvert.SerializeObject(UPC);

                    Exports["ghmattimysql"].execute("UPDATE characters SET coords=? WHERE identifier=?", new[] { pos, sid });
                }
                catch { continue; }
            }
        }
    }*/
}
