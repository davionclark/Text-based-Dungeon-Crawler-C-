using System.Collections;
using System.Collections.Generic;
using System;
using System.Dynamic;

namespace StarterGame
{
    public enum ItemType
    {
        Key, Weapon
    }

    public abstract class Item 
    {
        public virtual string Name { get; set; }
        public virtual int Weight { get; set; }
        public virtual float Volume { get; set; }
        public ItemType Type { get; set; }
        public virtual bool CanBePickedUp { get; set; }
        public virtual string Description { get; set; }
        public virtual int SaleValue { get; set; }
        public virtual int BuyValue { get; set; }
        public virtual int Durability { get; set; }
        public virtual int DurabilityDepreciation { get; set; }
        public virtual int Damage { get; set; }
    }
}
