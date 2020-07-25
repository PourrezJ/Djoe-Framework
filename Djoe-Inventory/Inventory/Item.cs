using CitizenFX.Core;
using CitizenFX.Core.Native;
using RDResurrection_Server.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace RDResurrection_Server.Inventory
{
    public class Item : ICloneable
    {
        public ItemID Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Classes { get; set; }
        public double Weight { get; set; }

        public bool IsGiven { get; set; }
        public bool IsUsable { get; set; }
        public bool IsStackable { get; set; }
        public bool IsDropable { get; set; }

        public string Type { get; set; }
        public string Icon { get; set; }
        public double ItemPrice { get; set; }
        public bool IsDockable { get; set; }

        public Dictionary<string, dynamic> Variables = new Dictionary<string, dynamic>();

        public Item(ItemID id, string name, string description, double weight = 0, bool isGiven = false, bool isUsable = false, bool isStackable = true, bool isDropable = true, bool isDockable = false, double itemPrice = 0,
            string type = "item", string icon = "unknown-item", string classes = "basic")
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Weight = weight;
            this.IsGiven = isGiven;
            this.IsUsable = isUsable;
            this.IsStackable = isStackable;
            this.Type = type;
            this.Icon = icon;
            this.Classes = classes;
            this.IsDropable = isDropable;
            this.IsDockable = isDockable;
            this.ItemPrice = itemPrice;
        }

        public virtual void Use(Player player, string inventoryType, int slot)
        {
        }


        public virtual void OnPlayerGetItem(Player player)
        {
        }

        public virtual bool Drop(Player player, int quantite, int slot, OutfitInventory inventory)
        {
            /*
            if (!IsDropable)
                return false;

            if (inventory.Delete(slot, quantite))
            {
                var position = c.GetPosition();
                var dimension = c.Dimension;
                ResuPickup pickup = ResuPickup.CreatePickup("prop_money_bag_01", this, quantite, new Vector3(position.X, position.Y, position.Z - 1), false, TimeSpan.FromMinutes(1), dimension);
                pickup.OnTakePickup += OnPickup;

                return true;
            }
            */
            return false;
        }

        public virtual bool Drop(Player player, int quantite, int slot, Inventory inventory)
        {
            Logger.Info("drop called");
            if (!IsDropable)
                return false;
            
            if (inventory.Delete(slot, quantite))
            {
                Logger.Info("delete item");

                var ph = player.GetPlayerHandler();

                if (ph == null)
                    return false;
               
                // TODO Create streamer ...
                
                //ResuPickup resu = ResuPickup.CreatePickup("prop_money_bag_01", this, quantite, new Vector3(position.X, position.Y, position.Z - 1), false, TimeSpan.FromMinutes(1), dimension);
                //resu.OnTakePickup += OnPickup;
                
                return true;
            }

            return false;
        }
        /*
        public virtual Task OnPickup(IPlayer client, ResuPickup pickup)
        {
            Entities.Players.PlayerHandler ph = client.GetPlayerHandler();

            if (ph != null)
            {
                if (!ph.InventoryIsFull(pickup.Quantite * pickup.Item.weight))
                {
                    if (ph.AddItem(pickup.Item, pickup.Quantite))
                    {
                        //client.PlayAnimation("putdown_low", "pickup_object", 49);
                        pickup.Delete();
                    }
                    else
                        client.SendNotificationError("Action impossible.");
                }
                else
                    client.SendNotificationError("Vous n'avez pas la place.");
            }

            return Task.CompletedTask;
        }*/
        /*
        public virtual Task Give(IPlayer sender, IPlayer recever, int quantite)
        {
            return Task.CompletedTask;
        }*/

        public void SetData(string key, object value) => Variables.Add(key, value);
        public dynamic GetData(string key) => Variables[key];
        public void ResetData(string key) => Variables[key] = null;
        public bool HasData(string key) => Variables.ContainsKey(key);

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        public Item CloneItem()
        {
            return (Item)MemberwiseClone();
        }
    }
}