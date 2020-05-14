using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    class AttackCommand : Command
    {
        BattleSystem battle = new BattleSystem();
        //private Notification notification;
        Dictionary<string, object> userInfo = new Dictionary<string, object>();

        public AttackCommand() : base()
        {
            this.name = "attack";
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
                if (player.inBattle)
                {
                    battle.attack();
                }
                else
                {
                    player.outputMessage("\nYou cannot attack outside of battle.");
                }
            }
            return false;
        }
    }
}

