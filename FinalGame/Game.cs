using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    public class Game
    {
        Player player;
        NPC npc;
        private NPC npc2;
        Parser parser;
        bool playing;

        public Game()
        {
            playing = false;
            parser = new Parser(new CommandWords());
            player = new Player(GameWorld.instance.Entrance);
            npc = new NPC(GameWorld.instance.NPCRoom, "Hulking Brute", "\nA hulking monster stares at you from across the room.", 10, 2); //Creates a new NPC.
            npc2 = new NPC(GameWorld.instance.BossRoom,"Eldritch Abomination","\nA horrifying creature glowing with purple energy makes its way towards you.",40, 5);
        }


        /**
     *  Main play routine.  Loops until end of play.
     */
        public void play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.

            bool finished = false;
            while (!finished)
            {
                Console.Write("\n>");
                Command command = parser.parseCommand(Console.ReadLine());
                if (command == null)
                {
                    Console.WriteLine("I don't understand...");
                }
                else
                {
                    finished = command.execute(player);
                }

                if (player.Hp <= player.MinimumHp) //Ends game if player health falls below 0.
                {
                    finished = true;
                }
                if (player.currentRoom == GameWorld.instance.End) //Ends game if player enters the end room.
                {
                    finished = true;
                }
            }
        }


        public void start()
        {
            playing = true;
            player.outputMessage(welcome());
        }

        public void end()
        {
            playing = false;
            player.outputMessage(goodbye());
        }

        public string welcome()
        {
            return "Welcome to Eldritch Labs, a text-based dungeon crawler.\n\nType 'help' for a list of commands. " +
                   "\n\nYou open your eyes and find yourself lying on a surgical table. " +
                   "You quickly realize that you have no clue where you are or who you are. " +
                   "You glance around the room briefly, there is a door leading to the north."; ;
        }

        public string goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}
