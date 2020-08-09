﻿using CitizenFX.Core;
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
            EventHandlers["API_GiveWeapon"] += new Action<string, int, bool, int, bool, float>(GiveWeapon);
            EventHandlers["API_PlayAnimation"] += new Action<string>(PlayAnimation);
            EventHandlers["API_PlayScenario"] += new Action<string>(PlayScenario);
            EventHandlers["API_ClearSecondary"] += new Action(ClearSecondary);
            EventHandlers["API_SetToWaypoint"] += new Action(SetToWaypoint);
            EventHandlers["API_SetPlayerPos"] += new Action<float, float, float, float>(SetPlayerPos);
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
        
        private static void GiveWeapon(string weapName, int ammoCount, bool equip, int group, bool leftHanded, float condition)
        {
            Debug.WriteLine("Give Weapon");
            Function.Call((Hash)0x5e3bddbcb83f3d84, Game.PlayerPed.Handle, Game.GetHashKey(weapName), ammoCount, equip, true, group, true, 0.5, 1.0, leftHanded, condition);
        }
    }
}
