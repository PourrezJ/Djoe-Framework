using CitizenFX.Core;
using Server.Utils;

namespace Server.Scripts
{
    public static class Survival
    {
        private static bool hunger;

        public static void Init() 
        {
            Misc.SetInterval(() => {
                hunger = !hunger;
                foreach (var ph in PlayerManager.Characters)
                {
                    if (ph.Value.Client == null)
                        continue;

                    var client = ph.Value.Client as Player;

                    if (hunger && ph.Value.Hunger > 0)
                        ph.Value.Hunger--;

                    if (ph.Value.Thirst > 0)
                        ph.Value.Thirst--;

                    if ((ph.Value.Hunger <= 0 || ph.Value.Thirst <= 0) && !ph.Value.IsDead)
                    {
                        //await AltAsync.Do(() => ph.SetHealth((ushort)(ph.Client.Health - 25)));
                    }
                    else
                        PlayerManager.UpdateUI(ph.Value);
                }
            }, 1000 * 60 * 3 / 2);
        }
    }
}
