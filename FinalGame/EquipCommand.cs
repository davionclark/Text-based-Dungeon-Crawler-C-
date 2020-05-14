using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    class EquipCommand : Command
    {
        public EquipCommand() : base()
        {
            this.name = "equip";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.equip(this.secondWord);
            }
            else
            {
                player.outputMessage("\nEquip what?");
            }
            return false;
        }
    }
}
