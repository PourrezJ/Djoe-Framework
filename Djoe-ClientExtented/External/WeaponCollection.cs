﻿using System.Collections.Generic;

namespace ClientExtented.External
{
    public sealed class WeaponCollection
    {
        private Ped Owner { get; }
        private readonly Dictionary<WeaponHash, Weapon> _weapons = new Dictionary<WeaponHash, Weapon>();

        internal WeaponCollection(Ped owner)
        {
            Owner = owner;
        }


    }
}
