using CitizenFX.Core;
using CitizenFX.Core.Native;
using ClientExtented;
using ClientExtented.External;
using Newtonsoft.Json;
using Shared;
using System;

namespace Client.Scripts
{
    public class NetworkAPI : BaseScript
    {
        public NetworkAPI()
        {
            EventHandlers["API_CreateVehicle"] += new Action<string, Vector3, float, bool>(CreateVehicle);
            EventHandlers["API_SetEntityAlpha"] += new Action<int>(SetEntityAlpha);
            EventHandlers["API_SetInvincible"] += new Action<bool>(SetInvincible);
            EventHandlers["API_SetInvisible"] += new Action<bool>(SetInvisible);
            EventHandlers["API_GiveWeapon"] += new Action<uint, int, bool, int, bool, float>(GiveWeapon);
            EventHandlers["API_PlayAnimation"] += new Action<string>(PlayAnimation);
            EventHandlers["API_PlayScenario"] += new Action<string>(PlayScenario);
            EventHandlers["API_ClearSecondary"] += new Action(ClearSecondary);
            EventHandlers["API_SetToWaypoint"] += new Action(SetToWaypoint);
            EventHandlers["API_SetPlayerPos"] += new Action<float, float, float, float>(SetPlayerPos);
            EventHandlers["API_RemoveAllWeapons"] += new Action(API_RemoveAllWeapons);
            EventHandlers["API_RemoveWeapon"] += new Action<uint>(API_RemoveWeapon);
        }

        private void API_RemoveAllWeapons()
        {
            API.RemoveAllPedWeapons(Game.PlayerPed.Handle, true, true);
        }

        private void API_RemoveWeapon(uint weaponHash)
        {
            API.RemoveWeaponFromPed(Game.PlayerPed.Handle, weaponHash, true, 0);
        }

        private void SetPlayerPos(float x, float y, float z, float heading)
        {
            Function.Call((Hash)0x203BEFFDBE12E96A, Game.PlayerPed.Handle, x, y, z, heading, false, false, false);
        }

        private async void SetToWaypoint()
        {
            if (World.IsWaypointActive)
            {
                Screen.FadeOut(0);
                var wayPointPos = await Misc.ForceGroundZ(World.WaypointPosition);
                await Delay(500);
                Game.PlayerPed.PositionNoOffset = wayPointPos;
                Screen.FadeIn(500);
            }
        }

        private void ClearSecondary()
        {
            Game.PlayerPed.Tasks.ClearSecondary();
        }

        private void PlayScenario(string anim)
        {
            Game.PlayerPed.Tasks.StartScenarioInPlace(anim);
        }

        private void PlayAnimation(string animData)
        {
            var anim = JsonConvert.DeserializeObject<AnimationsSync>(animData);

            Game.PlayerPed.Tasks.PlayAnimation(anim.AnimDict, anim.AnimName, anim.BlendInSpeed, anim.Duraction, anim.Flag);
        }

        private void SetInvisible(bool obj)
        {
            Game.PlayerPed.IsVisible = false;

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
        
        private static void GiveWeapon(uint weaponHash, int ammoCount, bool equip, int group, bool leftHanded, float condition)
        {

            //Function.Call(Hash._GIVE_WEAPON_TO_PED_2, Game.PlayerPed.Handle, Game.GetHashKey(weapName), ammoCount, equip, true, group, true, 0.5, 1.0, leftHanded, condition);

            //uint weaponHash = (uint)WeaponHash.RepeaterCarbine;

            leftHanded = Function.Call<bool>((Hash)0xD955FEE4B87AFA07, weaponHash);

            Debug.WriteLine($"Give Weapon: {weaponHash} {leftHanded}");

            Game.PlayerPed.GiveWeapon(weaponHash, ammoCount, equip, leftHanded, condition);
        }
    }
}
