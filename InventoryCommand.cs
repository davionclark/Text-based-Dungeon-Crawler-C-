using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class InventoryCommand : Command
    {

        public InventoryCommand() : base()
        {
            this.name = "inventory";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.outputMessage("\nThis is a one word command.");
            }
            else
            {
                player.inventoryList();
            }
            return false;
        }
    }
}