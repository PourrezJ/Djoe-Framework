﻿using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ClientExtented.External
{
    public class Ped : Entity
    {
        public Tasks Tasks { get; }
        public Ped(int handle) : base(handle)
        {
            Tasks = new Tasks(this);
        }

        public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;
        public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, Handle);
        public bool IsInMeleeCombat => Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, Handle);
        public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle);
        public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, Handle);
        public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, Handle);
        public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, Handle);
        public bool IsFleeing => Function.Call<bool>(Hash.IS_PED_FLEEING, Handle);
        public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, Handle);
        public bool IsGettingIntoVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, Handle);
        public bool IsInVehicle => Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Handle);
        public bool IsOnFoot => Function.Call<bool>(Hash.IS_PED_ON_FOOT, Handle);
        public bool IsOnMount => Function.Call<bool>(Hash.IS_PED_ON_MOUNT, Handle);
        public bool IsAttachedToAnyPed => Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_PED, Handle);
        public bool IsPedArrestingAnyPed => Function.Call<bool>((Hash)0xA9CC7856D52DBD25, Handle);
        public bool IsPedDuelling => Function.Call<bool>((Hash)0xC8B29D18022EA2B7, Handle);
        public bool IsPedBeingHogtied => Function.Call<bool>((Hash)0xD453BB601D4A606E, Handle);

        public bool IsSittingInVehicle() => Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
        public bool IsSittingInVehicle(Vehicle vehicle) => Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Handle, vehicle.Handle);

        public Ped GetMount => (Ped)FromHandle(Function.Call<int>(Hash.GET_MOUNT, Handle));
        public Vehicle CurrentVehicle => (Vehicle)FromHandle(Function.Call<int>(Hash.GET_VEHICLE_PED_IS_IN, Handle, false));
        public Vehicle LastVehicle => (Vehicle)FromHandle(Function.Call<int>(Hash.GET_VEHICLE_PED_IS_IN, Handle, true));

        public void SetMount(Ped ped, int seat, bool unk = false)
        {
            Function.Call((Hash)0x028F76B6E78246EB, Handle, ped, seat, unk);
        }

        public int HealthCore
        {
            get => GetCoreValue(PedCore.Health);
            set => SetCoreValue(PedCore.Health, value);
        }

        public int StaminaCore
        {
            get => GetCoreValue(PedCore.Stamina);
            set => SetCoreValue(PedCore.Stamina, value);
        }

        public int DeadEyeCore
        {
            get => GetCoreValue(PedCore.DeadEye);
            set => SetCoreValue(PedCore.DeadEye, value);
        }

        public float Scale
        {
            set => Function.Call((Hash)0x25ACFC650B65C538, Handle, value);
        }

        public string PromptName
        {
            set => Function.Call(Hash._SET_PED_PROMPT_NAME, Handle, value);
        }

        public bool BlockPermanentEvents
        {
            set => Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
        }

        public Weapon CurrentWeaponLeftHand
        {
            get {
                var wpnOut = new OutputArgument();
                Function.Call(Hash.GET_CURRENT_PED_WEAPON, Handle, wpnOut, true, true);
                return new Weapon(this, wpnOut.GetResult<WeaponHash>());
            }
        }
        public Weapon CurrentWeaponRightHand
        {
            get {
                var wpnOut = new OutputArgument();
                Function.Call(Hash.GET_CURRENT_PED_WEAPON, Handle, wpnOut, true, false);
                return new Weapon(this, wpnOut.GetResult<WeaponHash>());
            }
        }


        public int Outfit
        {
            set => Function.Call((Hash)0x77ff8d35eec6bbc4, Handle, value, 0);
        }

        public int GetCoreValue(PedCore core)
        {
            return Function.Call<int>((Hash)0x36731AC041289BB1, Handle, (int)core);
        }

        public bool GetConfigFlag(int flagID)
        {
            return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, flagID, true);
        }

        public void SetConfigFlag(int flagID, bool value)
        {
            Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, flagID, value);
        }

        public void ResetConfigFlag(int flagID)
        {
            Function.Call(Hash.SET_PED_RESET_FLAG, Handle, flagID, true);
        }

        public void SetCoreValue(PedCore core, int value)
        {
            Function.Call((Hash)0xc6258f41d86676e0, Handle, (int)core, MathUtil.Clamp(value, 0, 100));
        }
        //( int ped, Hash weapName, int ammoCount, BOOL equipNow, BOOL p4, int group, BOOL p6, int p7, int p8, BOOL leftHanded )
        public async void GiveWeapon(uint weapon, int ammoCount, bool equipNow = false, bool isLeftHanded = false, float condition = 0.0f)
        {
            var model = new Model((int)weapon);

            await model.Request(1000);
            Function.Call((Hash)0x5E3BDDBCB83F3D84, Handle, (uint)weapon, ammoCount, equipNow, true, 1, false, 0.5f, 1f, 752097756, isLeftHanded, condition);
            /*
            if (weapon == Game.GenerateHash("WEAPON_MELEE_LANTERN_ELECTRIC"))
            {
                Debug.WriteLine("GO PUTAIN DE LANTERN!");
                Function.Call((Hash)0xB282DC6EBD803C75, Handle, weapon, 500, true, 0);
            }
            else
            {
                Function.Call((Hash)0x5E3BDDBCB83F3D84, Handle, (uint)weapon, ammoCount, equipNow, true, 752097756, false,
                0.5f, 1f, 752097756, isLeftHanded, condition);
            }*/

        }

        public void SetIntoVehicle(Vehicle vehicle, VehicleSeat seat)
        {
            Function.Call(Hash.SET_PED_INTO_VEHICLE, Handle, vehicle.Handle, seat);
        }
        public void ResurrectPed() => Function.Call(Hash.RESURRECT_PED, Handle);
        public void SetEntityCoordsAndHeading(Vector3 position, float heading, bool xAxis = false, bool yAxis = false, bool zAxis = false) 
            => Function.Call((Hash)0x203BEFFDBE12E96A, Handle, position.X, position.Y, position.Z, heading, xAxis, yAxis, zAxis);

        public Relationship GetRelationshipWithPed(Ped ped)
        {
            return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped.Handle);
        }

        public RelationshipGroup RelationshipGroup
        {
            get => new RelationshipGroup(Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle));
            set => Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, Handle, value.Hash);
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum PedCore
    {
        Health = 0,
        Stamina,
        DeadEye
    }

}
