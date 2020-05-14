using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StarterGame
{
    class NPC
    {
        public List<Room> rooms = new List<Room>();
        private Room _currentRoom;
        Random rnd = new Random();
        private Room npcRoom;
        private int index;
        Dictionary<string, object> userInfo = new Dictionary<string, object>();
        private string name;
        private int minimumHP = 0;
        private int maximumHP = 10;
        private int hp;

        public int MaximumHp
        {
            get { return maximumHP; }
            set { maximumHP = value; }
        }

        public int MinimumHP
        {
            get { return minimumHP; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }

        private int damage = 2;
        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }
        private List<string> directions = new List<string> //The directions the NPC can move in.
        {
            "north", "east", "west", "south", "none"
        };

        private string description = "\nA hulking creature stares at you from across the room.";
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        private string direction;

        public Room currentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                _currentRoom = value;
            }
        }
        public NPC(Room room, string name) : this(room, name, "A horrific creatures stares at you from accross the room.", 10, 2)
        {

        }
        public NPC(Room room, string name, string description, int maximumHp, int damage)
        {
            this.Name = name;
            this.Description = description;
            this.MaximumHp = maximumHp;
            this.Damage = damage;
            _currentRoom = room;
            hp = this.MaximumHp;
            NotificationCenter.Instance.addObserver("PlayersCurrentRoom", playersCurrentRoom); //Checks the player's current room.
        }

        public void outputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public Room walkAround(string direction) //Allows the NPC to move.
        {
            Door door = this._currentRoom.getExit(direction);
            if (door != null)
            {
                if (door.Open)
                {
                    rooms.Add(_currentRoom);
                    this._currentRoom = door.room(this.currentRoom);
                }
            }
            return this.currentRoom;
        }

        public void playersCurrentRoom(Notification notification)
        {
            if (notification.userInfo != null)
            {
                Room room = (Room)notification.userInfo["PlayersCurrentRoom"];
                Player player = (Player)notification.Object;
                if (notification.userInfo.ContainsKey("PlayersCurrentRoom"))
                {
                    if (!userInfo.ContainsKey("PlayerFighting"))
                    {
                        userInfo.Add("PlayerFighting", player);
                    }
                    else
                    {
                        userInfo["PlayerFighting"] = player;
                    }
                    if (this._currentRoom == player.currentRoom) //Checks if player moved into the same room as the NPC.
                    {
                        if (this.Hp > this.MinimumHP)
                        {
                            //Initiates battle if the NPC is still alive.
                            player.outputMessage(this.Description);
                            player.outputMessage("\nThe " + this.Name + " attacks you. Fight for your life!");
                            player.InBattle = true;
                            NotificationCenter.Instance.postNotification(new Notification("PlayerFighting", this, userInfo));
                        }
                    }
                    else
                    {
                        //If the player moved and isn't in the same room as the monster, the monster then moves. Checks if they are now in the same room and initiates battle.
                        index = rnd.Next(directions.Count);
                        direction = directions[index];
                        npcRoom = this.walkAround(direction); //Makes the NPC walk in a random direction take from the directions list.
                        if (npcRoom == player.currentRoom)
                        {
                            if (this.Hp > this.MinimumHP) //Checks if NPC is still alive.
                            {
                                player.outputMessage(this.Description);
                                player.outputMessage("\nThe " + this.Name + " attacks you. Fight for your life!");
                                player.InBattle = true;
                                NotificationCenter.Instance.postNotification(new Notification("PlayerFighting", this, userInfo));
                            }
                        }
                    }
                }
            }
        }
    }
}
