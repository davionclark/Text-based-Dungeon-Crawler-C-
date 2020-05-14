using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    class BattleSystem
    {
        private bool attacking;
        private Notification attackNotification;

        public BattleSystem()
        {
            NotificationCenter.Instance.addObserver("PlayerFighting", playerFighting); //Adds an observer to see if the player is fighting a monster. Primary notification is from the NPC class.
        }

        public void attack() //Initiated when the player used the AttackCommand. Sends a notification to the playerFighting method.
        { 
            attacking = true;
            playerFighting(attackNotification);
        }

        public void playerFighting(Notification notification) //Initiated from the NPC class. Will not attack NPC until the attack command is used.
        {
            if (notification.userInfo != null)
            {
                Player player = (Player)notification.userInfo["PlayerFighting"];
                NPC npc = (NPC)notification.Object;
                if (notification.userInfo.ContainsKey("PlayerFighting"))
                {
                    if (attacking == true) //If player uses the attack command.
                    {
                        npc.Hp -= player.EquippedWeapon.Damage; //Decreases npc health based on the damage of the player's weapon.

                        player.outputMessage("\nYou strike the " + npc.Name + " for " + player.EquippedWeapon.Damage + " damage with your " + player.EquippedWeapon.Name + ". It has " + npc.Hp + " health remaining.");
                        player.EquippedWeapon.Durability -= player.EquippedWeapon.DurabilityDepreciation; //Decreases player's weapon durability based on the durability depreciation value.
                        if (player.EquippedWeapon.Durability <= 0) //If durability falls below 0.
                        {
                            player.outputMessage("\nYour " + player.EquippedWeapon.Name + " breaks in your hands.");
                            player.removeWeapon();
                        }

                        if (npc.Hp <= npc.MinimumHP) //If the NPC's health falls below the minimum value.
                        {
                            //Lets player know they killed the monster. Restores player HP to max. Ends battle and sets the NPC to null.
                            player.outputMessage("\nYou slay the " + npc.Name + ". Your health is restored.");
                            npc = null;
                            player.Hp = player.MaximumHp;
                            attacking = false;
                            player.inBattle = false;
                            return;
                        }

                        //Simulates the NPC attacking the player.
                        player.Hp -= npc.Damage; //Decreases the player's health by the NPC's damage.
                        player.outputMessage("\nThe " + npc.Name + " hits you for " + npc.Damage + " damage. You have " + player.Hp + " health remaining.");
                        if (player.Hp <= player.MinimumHp) //If player's health falls below the minimum value.
                        {
                            //Prints message and exits the value. The Game class will end the game once this happens.
                            player.outputMessage("\nYou have been slain.");
                            attacking = false;
                            player.inBattle = false;
                            return;
                        }
                    }

                    attackNotification = notification; //Allows the attack command to execute the playerFighting method again.
                    attacking = false; //Ends attack even if no one died.
                }
            }
        }
    }
}
