using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class OpenCommand : Command
    {

        public OpenCommand() : base()
        {
            this.name = "open";
        }

        override
        public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                if (this.hasThirdWord())
                {
                    player.open(this.secondWord, this.thirdWord);
                }
                else
                {
                    player.outputMessage("\nOpen " + this.secondWord + " where?");
                }
            }
            else
            {
                player.outputMessage("\nOpen what?");
            }
            return false;
        }
    }
}
