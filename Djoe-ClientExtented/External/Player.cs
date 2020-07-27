using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace ClientExtended.External
{
    public sealed class Player : INativeValue, IEquatable<Player>
    {
        public int Handle { get; private set; }

        private Ped _ped;

        public Player(int handle)
        {
            Handle = handle;
        }

        public bool Exists()
        {
            return Function.Call<bool>(Hash.NETWORK_IS_PLAYER_ACTIVE, Handle);
        }

        public Ped Character
        {
            get {
                if (!ReferenceEquals(_ped, null) && _ped.Handle == Handle)
                {
                    return _ped;
                }
                _ped = new Ped(API.GetPlayerPed(Handle));
                return _ped;
            }
        }

        public string Name => Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);

        public static int Money
        {
            get => Function.Call<int>((Hash)0x0C02DABFA3B98176);
            set
            {
                var source = Money;
                var target = value;
                if (target < source)
                {
                    Function.Call((Hash)0x466BC8769CF26A7A, source - target);
                }
                else
                {
                    Function.Call((Hash)0xBC3422DC91667621, target - source);
                }
            }
        }

        public int WantedLevel
        {
            get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
            set
            {
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
            }
        }

        public bool IsDead => Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);

        public bool IsAlive => !IsDead;

        public bool IsAiming => Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);

        public bool IsClimbing => Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);

        public bool IsRidingTrain => Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);


        public bool IsPlaying => Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);

        public bool IsInvincible
        {
            get => Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
            set => Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
        }

        public bool IgnoredByEveryone
        {
            set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
        }

        public bool CanUseCover
        {
            set => Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
        }

        public bool CanStartMission
        {
            get => Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);
        }

        public bool CanControlCharacter
        {
            get => Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
            set => Function.Call(Hash.SET_PLAYER_CONTROL, Handle, value, 0, 0);
        }
        public int ServerId => Function.Call<int>(Hash.GET_PLAYER_SERVER_ID, Handle);

        public async Task<bool> ChangeModel(Model model, int timeout = 3000)
        {
            if (!model.IsInCdImage || !model.IsPed || !await model.Request(timeout))
            {
                return false;
            }

            Function.Call(Hash.SET_PLAYER_MODEL, Handle, model.Hash, false);
            Function.Call((Hash)0x283978A15512B2FE, Character.Handle, true);
            Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, model.Hash);

            return true;
        }


        public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_PLAYERS_LAST_VEHICLE);

        public bool IsTargetting(Entity entity)
        {
            return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle, 0);
        }

        public bool IsTargettingAnything => Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);


        public void DisableFiringThisFrame(bool toggle)
        {
            Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, toggle);
        }

        public void SetSuperJumpThisFrame()
        {
            Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Handle);
        }

        public void SetMayNotEnterAnyVehicleThisFrame()
        {
            Function.Call(Hash.SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
        }

        public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle)
        {
            Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle);
        }


        public bool Equals(Player other)
        {
            return other != null && ReferenceEquals(this, other) && other.Handle == Handle;
        }

        public override bool Equals(object obj)
        {
            return obj is Player other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override ulong NativeValue
        {
            get => (ulong)Handle;
            set => Handle = unchecked((int)value);
        }
    }
}
