using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Utils.Extensions;
using ClientExtended.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Scripts
{
    public class NetworkAPI : BaseScript
    {
        public NetworkAPI()
        {
            EventHandlers["API_CreatePed"] += new Action<string, Vector3, float, bool>(CreatePed);
            EventHandlers["API_CreateVehicle"] += new Action<string, Vector3, float, bool>(CreateVehicle);
            EventHandlers["API_SetEntityAlpha"] += new Action<int>(SetEntityAlpha);
            EventHandlers["API_SetInvincible"] += new Action<bool>(SetInvincible);
            EventHandlers["API_SetInvisible"] += new Action<bool>(SetInvisible);
            EventHandlers["API_GiveWeapon"] += new Action<string, int, bool, int, bool, float>(GiveWeapon);
        }

        private void SetInvisible(bool obj)
        {
            Game.PlayerPed.IsVisible = false;

        }

        private async void CreatePed(string pedHash, Vector3 position, float heading, bool isNet)
        {
            await World.CreatePed(pedHash, Game.PlayerPed.Position.Forward(0, 2), Game.PlayerPed.Heading);
        }

        private async void CreateVehicle(string model, Vector3 position, float heading, bool isNet)
        {
            await World.CreateVehicle(model, position, heading, isNet);
        }

        private void SetEntityAlpha(int alpha)
        {
            Game.PlayerPed.Alpha = alpha;
        }

        private void SetInvincible(bool invisible)
        {
            Game.PlayerPed.IsInvincible = invisible;
        }
        
        private static void GiveWeapon(string weapName, int ammoCount, bool equip, int group, bool leftHanded, float condition)
        {
            Debug.WriteLine("Give Weapon");
            Function.Call((Hash)0x5e3bddbcb83f3d84, Game.PlayerPed.Handle, Game.GetHashKey(weapName), ammoCount, equip, true, group, true, 0.5, 1.0, leftHanded, condition);
        }
    }
}
