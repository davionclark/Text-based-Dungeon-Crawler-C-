using System;
using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class IdCard: Item
    {
        public int KeyID { get; set; }

        public IdCard(string name, int weight, bool canBePickedUp, int keyID, float volume) : this(name, weight, canBePickedUp, keyID, volume, " You notice an id card laying in the corner of the room.")
        {

        }

        public IdCard(string name, int weight, bool canBePickedUp, int keyID, float volume, string description)
        {
            this.Name = name;
            this.Weight = weight;
            this.Type = ItemType.Key;
            this.CanBePickedUp = canBePickedUp;
            this.Description = description;
            this.KeyID = keyID;
            this.Volume = volume;
            this.Description = description;
        }

        //private string description = " You notice an id card laying in the corner of the room."; //Default description
        //public override string Description
        //{
        //    get { return description; }
        //    set { description = value; }
        //}
    }
}