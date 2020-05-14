using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    public class SellCommand : Command
    {

        public SellCommand() : base()
        {
            this.name = "sell";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.sell(this.secondWord);
            }
            else
            {
                player.outputMessage("\nSell what?");
            }
            return false;
        }
    }
}
