using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    public class LookCommand : Command
    {

        public LookCommand() : base()
        {
            this.name = "look";
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
                player.outputMessage(player.currentRoom.altDescription());
            }
            return false;
        }
    }
}

