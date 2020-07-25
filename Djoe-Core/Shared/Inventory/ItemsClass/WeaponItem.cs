namespace Shared.ItemsClass
{
    public class WeaponItem : Item
    {
        public int WeaponHash;

        public WeaponItem(ItemID id, string name, string description, double weight = 0, bool isGiven = false, bool isUsable = false, bool isStackable = true, bool isDropable = true, double itemPrice = 0, string icon = "unknown-item") : base(id, name, description, weight, isGiven, isUsable, isStackable, isDropable, itemPrice, icon)
        {
        }
    }
}
