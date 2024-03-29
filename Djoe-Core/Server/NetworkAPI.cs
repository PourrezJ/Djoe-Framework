﻿using CitizenFX.Core;
using Server.Utils.Enums;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class NetworkAPI
    {
        /*
        public static void CreatePed(Player player, string model, Vector3 position, float heading = 0, bool isNet = true)
        {
            player.TriggerEvent("API_CreatePed", model.ToString(), position, heading, isNet);
        }
        */
        public static void CreateVehicle(Player player, string model, Vector3 position, float heading = 0, bool isNet = true)
        {
            player.TriggerEvent("API_CreateVehicle", model.ToString(), position, heading, isNet);
        }

        public static void SetAlpha(Player player, int alpha)
        {
            player.TriggerEvent("API_SetEntityAlpha", alpha); 
        }

        public static void SetInvincible(Player player, bool invincible)
        {
            player.TriggerEvent("API_SetInvincible", invincible);
        }

        public static void SetInvisible(Player player, bool invisible)
        {
            player.TriggerEvent("API_SetInvisible", invisible);
        }

        public static void GiveWeapon(Player player, uint weapHash, int ammoCount, bool equip, int group, bool leftHanded, float condition)
        {
            player.TriggerEvent("API_GiveWeapon", weapHash, ammoCount, equip, group, leftHanded, condition);
        }

        public static void SetNoClip(Player player, bool active)
        {
            player.TriggerEvent("API_SetNoClip", active);
        }

        public static void SetToWaypoint(Player player)
        {
            player.TriggerEvent("API_SetToWaypoint");
        }

        public static void SetPlayerPos(Player player, UCoords pos)
        {
            player.TriggerEvent("API_SetPlayerPos", pos.X, pos.Y, pos.Z, pos.Heading);
        }

        public static void RemoveAllWeapons(Player player)
        {
            player.TriggerEvent("API_RemoveAllWeapons");
        }

        public static void RemoveWeapon(Player player, uint weapHash)
        {
            player.TriggerEvent("API_RemoveWeapon", weapHash);
        }
    }
}
