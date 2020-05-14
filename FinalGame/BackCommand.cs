using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class BackCommand : Command
    {

        public BackCommand() : base()
        {
            this.name = "back";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.outputMessage("This is a one word command.");
            }
            else
            {
                player.back();
            }
            return false;
        }
    }
}