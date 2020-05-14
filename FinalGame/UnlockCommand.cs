using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    class UnlockCommand : Command
    {
        public UnlockCommand() : base()
        {
            this.name = "unlock";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                if (this.hasThirdWord()) //Makes sure the player included a third word.
                {
                    player.unlock(this.secondWord, this.thirdWord);
                }
                else
                {
                    player.outputMessage("\nThis is a three word command.");
                }
            }
            else
            {
                player.outputMessage("\nThis is a three word command.");
            }
            return false;
        }
    }
}