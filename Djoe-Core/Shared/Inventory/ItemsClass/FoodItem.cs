using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Inventory.ItemsClass
{
    class FoodItem : Item
    {
        public int Thirst;
        public int Hunger;
        public int Stamina;
        public int Metabolism;
        public int InnerCoreHealth;
        public int OuterCoreHealth;
        public double InnerCoreHealthGold;
        public double OuterCoreHealthGold;
        public double InnerCoreStaminaGold;
        public double OuterCoreStaminaGold;
        public string PropName;
        public string Animation;

        public FoodItem(ItemID id, string name, string description, double weight = 0, bool isGiven = false, bool isUsable = false, bool isStackable = true, bool isDropable = true, double itemPrice = 0, string icon = "unknown-item") : base(id, name, description, weight, isGiven, isUsable, isStackable, isDropable, itemPrice, icon)
        {
        }
    }
}
