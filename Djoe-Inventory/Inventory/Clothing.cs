﻿using CitizenFX.Core;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RDResurrection_Server.Models
{
    public struct ClothData
    {
        public readonly int Drawable;
        public readonly int Texture;
        public readonly int Palette;

        public ClothData(int Drawable, int Texture, int Palette)
        {
            this.Drawable = Drawable;
            this.Texture = Texture;
            this.Palette = Palette;
        }

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PropData
    {
        public readonly int Drawable;
        public readonly int Texture;

        public PropData(int drawable, int texture)
        {
            Drawable = drawable;
            Texture = texture;
        }
    }
    public enum ClothSlot
    {
        Mask = 1,
        Hair = 2,
        Torso = 3,
        Legs = 4,
        Bags = 5,
        Feet = 6,
        Accessory = 7,
        Undershirt = 8,
        BodyArmor = 9,
        Decals = 10,
        Tops = 11
    }
    public enum PropSlot
    {
        Hats = 0,
        Glasses = 1,
        Ears = 2,
        Watches = 6,
        Bracelets = 7
    }
    public class Clothings
    {
        public ClothData Accessory = new ClothData(0, 0, 0);
        public ClothData Mask = new ClothData(0, 0, 0);
        public ClothData Legs = new ClothData(0, 0, 0);
        public ClothData Feet = new ClothData(1, 0, 0);
        public ClothData BodyArmor = new ClothData(0, 0, 0);
        public ClothData Bags = new ClothData(0, 0, 0);
        public ClothData Undershirt = new ClothData(0, 0, 0);
        public ClothData Tops = new ClothData(0, 0, 0);
        public ClothData Torso = new ClothData(15, 0, 0);

        public PropData? Hats;
        public PropData? Bracelets;
        public PropData? Ears;
        public PropData? Glasses;
        public PropData? Watches;

        [BsonIgnore]
        public Player Player;

        public Clothings(Player player)
        {
            Player = player;
        }

        public void UpdatePlayerClothing()
        {
            if (Player == null)
                return;

            var cloths = new Dictionary<ClothSlot, ClothData>()
            {
                { ClothSlot.Accessory, new ClothData(Accessory.Drawable, Accessory.Texture, Accessory.Palette) },
                { ClothSlot.Mask, new ClothData(Mask.Drawable, Mask.Texture, Mask.Palette) },
                { ClothSlot.Tops, new ClothData(Tops.Drawable, Tops.Texture, Tops.Palette) },
                { ClothSlot.Undershirt, new ClothData(Undershirt.Drawable, Undershirt.Texture, Undershirt.Palette) },
                { ClothSlot.Legs, new ClothData(Legs.Drawable, Legs.Texture, Legs.Palette) },
                { ClothSlot.Feet, new ClothData(Feet.Drawable, Feet.Texture, Feet.Palette) },
                { ClothSlot.BodyArmor, new ClothData(BodyArmor.Drawable, BodyArmor.Texture, BodyArmor.Palette) },
                { ClothSlot.Bags, new ClothData(Bags.Drawable, Bags.Texture, Bags.Palette) },
                { ClothSlot.Torso, new ClothData(Torso.Drawable, Torso.Texture, Torso.Palette) }
            };
            /*
            foreach (KeyValuePair<ClothSlot, ClothData> entry in cloths)
                Player.SetCloth(entry.Key, entry.Value.Drawable, entry.Value.Texture, entry.Value.Palette);

            if (Bracelets != null)
                Player.SetProp(PropSlot.Bracelets, Bracelets.Value);
            else
                Player.ClearProp(PropSlot.Bracelets);

            if (Ears != null)
                Player.SetProp(PropSlot.Ears, Ears.Value);
            else
                Player.ClearProp(PropSlot.Ears);

            if (Glasses != null)
                Player.SetProp(PropSlot.Glasses, Glasses.Value);
            else
                Player.ClearProp(PropSlot.Glasses);

            if (Watches != null)
                Player.SetProp(PropSlot.Watches, Watches.Value);
            else
                Player.ClearProp(PropSlot.Watches);

            if (Hats != null)
                Player.SetProp(PropSlot.Hats, Hats.Value);
            else
                Player.ClearProp(PropSlot.Hats);*/
        }
    }
}
