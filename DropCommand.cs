using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class DropCommand : Command
    {

        public DropCommand() : base()
        {
            this.name = "drop";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.drop(this.secondWord);
            }
            else
            {
                player.outputMessage("\nDrop what?");
            }
            return false;
        }
    }
}