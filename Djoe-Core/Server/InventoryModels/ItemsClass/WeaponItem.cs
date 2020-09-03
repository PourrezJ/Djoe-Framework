using CitizenFX.Core;
using Server.Utils;
using Shared;

namespace Server.ItemsClass
{
    public class WeaponItem : Item
    {
        public string HashName;

        public string WeaponModel;

        public int CurrentAmmo;

        public WeaponItem(ItemID id, string name, string description, double weight = 0, bool isGiven = false, bool isUsable = false, bool isStackable = true, bool isDropable = true, double itemPrice = 0, string icon = "unknown-item") : base(id, name, description, weight, isGiven, isUsable, isStackable, isDropable, itemPrice, icon)
        {
            Id = ItemID.Weapon;
            IsUsable = false;
            IsDropable = true;
        }

        public void Equip(Player player)
        {
            NetworkAPI.GiveWeapon(player, (uint)Misc.GetHashKey(HashName), CurrentAmmo, false, 0, false, 1);
        }

        public void UnEquip(Player player)
        {
            NetworkAPI.RemoveWeapon(player, (uint)Misc.GetHashKey(HashName));
        }
    }
}
