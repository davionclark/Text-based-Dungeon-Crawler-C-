using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StarterGame
{
    class Weapon : Item
    {
        public Weapon(string name, int weight, bool canBePickedUp, int durability, int damage, int buyValue, int saleValue, float volume, int durabilityDepreciation)
        {
            this.Name = name;
            this.Weight = weight;
            this.Type = ItemType.Weapon;
            this.CanBePickedUp = canBePickedUp;
            this.Description = description;
            this.Durability = durability;
            this.Damage = damage;
            this.BuyValue = buyValue;
            this.SaleValue = saleValue;
            this.Volume = volume;
            this.DurabilityDepreciation = durabilityDepreciation;
        }

        private string description = " You notice a weapon nearby."; //Default weapon description.
        public override string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
    }
}

