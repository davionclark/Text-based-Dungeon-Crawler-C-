using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    class UnequipCommand : Command
    {
        public UnequipCommand() : base()
        {
            this.name = "unequip";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.unequip(this.secondWord);
            }
            else
            {
                player.outputMessage("\nUnequip what?");
            }
            return false;
        }
    }
}
