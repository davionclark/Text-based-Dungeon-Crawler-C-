using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class GrabCommand : Command
    {

        public GrabCommand() : base()
        {
            this.name = "grab";
        }

        override
            public bool execute(Player player)
        {
            if (this.hasSecondWord())
            {
                player.grab(this.secondWord);
            }
            else
            {
                player.outputMessage("\nThis is a two word command.");
            }
            return false;
        }
    }
}