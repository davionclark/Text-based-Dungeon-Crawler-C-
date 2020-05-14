using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    public class BuyCommand : Command
    {

        public BuyCommand() : base()
        {
            this.name = "buy";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.buy(this.secondWord);
            }
            else
            {
                player.outputMessage("\nBuy what?");
            }
            return false;
        }
    }
}
