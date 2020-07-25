namespace Shared
{
    class Weapons : Item
    {
        public int WeaponHash;

        public Weapons(ItemID id, string name, string description, double weight = 0, bool isGiven = false, bool isUsable = false, bool isStackable = true, bool isDropable = true, bool isDockable = false, double itemPrice = 0, string icon = "unknown-item") : base(id, name, description, weight, isGiven, isUsable, isStackable, isDropable, isDockable, itemPrice, icon)
        {
        }
    }
}
