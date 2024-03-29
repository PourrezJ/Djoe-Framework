﻿using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ClientExtented.External
{
    public sealed class Blip : PoolObject, IEquatable<Blip>
    {        //Function.Call((Hash)0x662D364ABF16DE2F, blip, 453264060); purple

        public Blip(int handle) : base(handle)
        {
        }

        public BlipSprite Sprite
        {
            set => Function.Call(Hash.SET_BLIP_SPRITE, Handle, value, true);
        }

        public Vector2 Scale
        {
            set => Function.Call(Hash.SET_BLIP_SCALE, Handle, value.X, value.Y);
        }

        public string Label
        {
            set => Function.Call(Hash.SET_BLIP_NAME_FROM_TEXT_FILE, Handle,
                Function.Call<int>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", value));
        }

        public bool IsFlashing
        {
            set => Function.Call(Hash.SET_BLIP_FLASHES, Handle, value, 2);
        }

        public bool IsOnMinimap => Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);

        public override bool Exists()
        {
            return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
        }

        public override void Delete()
        {
            Function.Call(Hash.REMOVE_BLIP, Handle);
        }

        public bool Equals(Blip other)
        {
            return !ReferenceEquals(null, other) && other.Handle == Handle;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Blip other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }

    /// <summary>
    /// The sprite icon to set the blip to. Here's a
    /// <see href="https://cdn.discordapp.com/attachments/450373719974477835/643937562091716638/unknown.png">reference image</see>
    /// in order from left-to-right by GlitchDetector, and each row indicates the different type.
    /// </summary>
    public enum BlipSprite : uint
    {
        Dominoes = 0x9D9FE583,
        DominoesAllFives = 0xFD189BDE,
        DominosAllThrees = 0xA1C2EBE4,
        FiveFingerFillet = 0x75B54B90,
        FiveFingerFilletGuts = 0x7869CF4,
        FiveFingerFilletBurnout = 0x3C88E424,
        Poker = 0x4A2357A9,
        SaddleBag = 0xB0E5E617,
        Doctor = 0x984E7CA9,
        EatingUtensils = 0x37BEBE4E,
        DonateToCamp = 0x8B7E38C4,
        ObjectiveChore = 0xDDFBA6AB,
        Ammunition = 0x5DF6DEBD,
        HealthSupplies = 0xD68D851B,
        Provisions = 0x919BC110,
        SmallBlackDot = 0x4ECB0761,
        Wheel = 0x3C5469D5,
        Stranger = 0x935EE440,
        Drinking = 0x4A0E7F51,
        HuntingGrounds = 0x1DCFAA8C,
        Fishing = 0xA216510E,
        MoneyWheel = 0xD4859AFE,
        Bank = 0x25249A47,
        Herd = 0x193BD50E,
        CaravanCamp = 0xA0417C98,
        HomeRobbery = 0x1A7A040D
    }

    public enum BlipType : uint
    {
        CompanionGray = 0x19365607,
        PickupWhite = 0xEC972124,
        WeaponWhite = 0x63351D54,
        WhiteDot = 0xB04092F8,
        Flashing = 0x4B1C3939,
        EnemyPink = 0x9A7FB0BF,
        DestinationSmall = 0xC19DA63,
        DestinationGray = 0xD792CF71,
        PosseCamp = 0x5D0509CC,
        DestinationLarge = 0x1857A152,
        DestinationDark = 0x64994D7C
    }
    /*
    local BLIP_TYPES = {
    -678244495, -- gray destination
    -1702907713, -- pink enemy
    -1337945352, -- white unselectable?
    1560611276, -- posse camp
    -674883803, -- invisible?
    1687768444, -- destination, mine icon?
    1260140857, -- invisible?
    -1230993421, -- horse icon "Arthur's Horse"
    -936216634, -- white unselectable?
    2098831270, -- white no name?
    -325639900, -- white pickup
    -773443467, -- corpse
    -700928964, -- gray herd
    -214162151, -- yellow straggler
    -1594303955, -- animal carcass
    662885764, -- red lawman
    -1049390151, -- red witness
    953018525, -- red bounty target
    455691738, -- red bounty target
    -839369609, -- red enemy
    -1749618580, -- white companion
    168093330, -- white destination
    -1595050198, -- red lawman
    -118010418, -- red alternative? lawman
    522464759, -- red bounty hunter
    631964804, -- wagon icon "Arthur's Wagon"
    1055493006, -- small yellow (x) destination
    486881925, -- white hat
    203020899, -- small yellow (x) destination
    -399496385, -- train
    -89429847, -- light gray companion
    1664425300, -- white weapon
    831283580, -- red enemy
    422991367, -- dark gray companion
    -515518185, -- white saddle unselectable?
    2033377404, -- yellow ( ) unselectable?
    -308585968, -- yellow (x) destination
    -108658760, -- invisible?
    -666422021, -- red ( ) unselectable?
    1774867085, -- invisible?
    -1282792512, -- yellow ( ) unselectable?
    227551412, -- yellow ( ) unselectable?
    408396114, -- yellow (x) destination
    -1559907306, -- white ( ) unselectable?
    1673015813, -- white ( ) unselectable?
    1247852480, -- yellow ( ) unselectable?
}*/
}
