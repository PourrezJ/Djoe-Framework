using CitizenFX.Core;
using Server.Utils.Extensions;
using Shared;
using System;
using System.Threading.Tasks;

namespace Server.ItemsClass
{
    public class FoodItem : Item
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

        public override void Use(Player client, string inventoryType, int slot, int quantity = 0)
        {
            var pData = client.GetPlayerDatabase();

            if (pData == null)
                return;

            if (pData.DeleteItem(slot, inventoryType, 1))
            {
                if (pData.Hunger + Hunger > 100)
                    pData.Hunger = 100;
                else
                    pData.Hunger += Hunger;

                if (pData.Thirst + Thirst > 100)
                    pData.Thirst = 100;
                else
                    pData.Thirst += Thirst;


                pData.Update();
                pData.UpdateUI();

                if (!string.IsNullOrEmpty(Animation))
                {
                    client.PlayScenario(Animation);

                    Task.Run(async () =>
                    {
                        await BaseScript.Delay(5000);
                        client.ClearSecondaryTask();
                    });
                }

            }
        }
    }
}
