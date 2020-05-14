using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace StarterGame
{

    public class GameWorld
    {

        static private GameWorld _instance;

        static public GameWorld instance
        {
            get
            {
                if (_instance == null) _instance = new GameWorld();
                return _instance;
            }
        }
        
        private Room _npcRoom;
        private Room _entrance;
        private Room trigger;
        private Room teleportTrigger;
        private Room shopTrigger;
        private Room victoryTrigger;
        private Room bossRoom;
        private int index;
        private int npcIndex;
        private List<Room> teleportRooms = new List<Room>(); //List of rooms the teleport room can teleport you to.
        private List<Room> npcRooms = new List<Room>(); //List of rooms the NPC can spawn in.
        private Dictionary<Room, Item> salesList = new Dictionary<Room, Item>(); //Dictionary containing items that can be bought in each room.

        public Room NPCRoom
        {
            get { return _npcRoom; }
            set { _npcRoom = value; }
        }
        public Room Entrance
        {
            get { return _entrance; }
        }
        public Room End
        {
            get { return victoryTrigger; }
        }

        public Room BossRoom
        {
            get { return bossRoom; }
        }

        private GameWorld()
        {
            _entrance = createWorld();
            // GameWorld subscribes to the notification PlayerPickedUpItem & PlayerDroppedItem & PlayerBuyAttempt & PlayerEnteredRoom & PlayerSellAttempt
            NotificationCenter.Instance.addObserver("PlayerPickedUpItem", playerPickedUpItem);
            NotificationCenter.Instance.addObserver("PlayerDroppedItem", playerDroppedItem);
            NotificationCenter.Instance.addObserver("PlayerEnteredRoom", playerEnteredRoom);
            NotificationCenter.Instance.addObserver("PlayerBuyAttempt", playerBuyAttempt);
            NotificationCenter.Instance.addObserver("PlayerSellAttempt", playerSellAttempt);
        }

        private Room createWorld()
        {
            Random rnd = new Random();
            index = rnd.Next(teleportRooms.Count);
            npcIndex = rnd.Next(npcRooms.Count);

            string sludgeTag = "You are in a dimly lit hallway. Purple sludge trails across the floor and through the opened door to your east.";
            string altSludgeTag = "\nSections of the floor have giant furrows through them, almost as if they have been raked by giant claws.";

            string startTag = "You find yourself back in the room you woke up in. The broken light bulb still flickers ominously above.\n";
            string altStartTag = "\nYou are in a surgical room. Dim light flickers from the broken bulb above. A door exiting the room is to your north.";

            string transportTag = "You are in a room with strange, glowing symbols covering the walls. Your surroundings hum and vibrate with a mysterious energy. For some reason you get the feeling that something strange will happen if you try to leave.";

            string shopTag = "You find yourself in a room that contains what you can only describe as a futuristic vending machine. The machine has two slots, one for selling and one for buying.";
            string altShop = "\nThe vending machine has a picture of a gun on it. The label beneath shows an oddly shaped '10'. You could probably sell unwanted weapons here if you wanted.";

            string hallwayTag = "You step into a large foyer shrouded by gloom. You can faintly make out a stairway leading north.";

            string victoryTag = "You step out of the laboratory and into the sun. Glee fills your body as you realize you have finally escaped. Congratulations.";

            Room magicTransport = new Room(transportTag);
            Room startingRoom = new Room(startTag, altStartTag);
            Room sludgeHall = new Room(sludgeTag, altSludgeTag);
            Room shop = new Room(shopTag, altShop);
            Room hallway = new Room(hallwayTag);
            Room theEnd = new Room(victoryTag);

            Item idCard1 = new IdCard("id", 2, true, 1, 0.5f);
            Item key = new IdCard("key",2,true,2,2f," You notice a key laying atop a floor tile.");
            Item rock = new Weapon("rock",7,true,1,2,2,10,2f,1);
            rock.Description = " A loose rock rests on the floor.";

            Item scalpel = new Weapon("scalpel", 1,true, 3,3,15,10, 2f,1);
            Item gun = new Weapon("gun", 5, true,2,20,10,8, 3f,1);
            
            Door door4 = Door.connect(hallway,theEnd,"south","north");
            door4.Closed = true;
            bossRoom = hallway;

            Door door3 = Door.connect(sludgeHall, hallway, "south", "north");
            door3.Closed = true;
            door3.Lock(key);

            Door door2 = Door.connect(sludgeHall, shop, "west", "east");
            door2.Closed = true;
            door2.Lock(key);

            Door.connect(sludgeHall, magicTransport, "east", "west");

            Door door = Door.connect(startingRoom, sludgeHall, "south", "north");
            door.Closed = true; //Closes the door.
            door.Lock(idCard1); //Locks the door with a specific key.

            startingRoom.addItem(idCard1);
            shop.addItem(key);
            sludgeHall.addItem(rock);

            sludgeHall.roomID = 1;
            magicTransport.roomID = 99; //ID of the teleportation room.

            scalpel.Description = " You notice a scalpel laying in a puddle of sludge.";
            startingRoom.addItem(scalpel); //Adds the scalpel to the starting room.
            gun.Description = " A shiny pistol sits on the ground.";

            victoryTrigger = theEnd;
            trigger = startingRoom; //
            teleportTrigger = magicTransport;
            shopTrigger = shop;

            salesList.Add(shopTrigger,gun);

            teleportRooms.Add(shop);

            npcRooms.Add(sludgeHall); 
            npcRooms.Add(shop);
            if (npcRooms.Count != 0)
            {
                NPCRoom = npcRooms[npcIndex];
            }
            return startingRoom;

        }
        // callback method for PlayerPickedUpItem
        public void playerPickedUpItem(Notification notification)
        {
            Item item = (Item)notification.userInfo["PlayerPickedUpItem"];
            Player player = (Player)notification.Object;
            trigger = player.currentRoom;
            if (notification.userInfo.ContainsKey("PlayerPickedUpItem"))
            {
                notification.userInfo.Remove("PlayerPickedUpItem");
            }
        }  
        //Callback method for PlayerDroppedItem
        public void playerDroppedItem(Notification notification)
        {
            Item item = (Item)notification.userInfo["PlayerDroppedItem"];
            Player player = (Player)notification.Object;
            trigger = player.currentRoom;
            if (notification.userInfo.ContainsKey("PlayerDroppedItem"))
            {
                notification.userInfo.Remove("PlayerDroppedItem");
            }
        }

        //Call back method for PlayerEnteredRoom
        public void playerEnteredRoom(Notification notification)
        {
            Room room = (Room) notification.userInfo["PlayerEnteredRoom"];
            Player player = (Player)notification.Object;

            if (notification.userInfo.ContainsKey("PlayerEnteredRoom"))
            {
                if (room == teleportTrigger) //If player is in the telportation room.
                {
                    player.IsTeleporting = true;
                    player.outputMessage("\nYour vision swirls and your balance shifts. When you regain your sight you realize you've been transported somewhere new.");
                    player.currentRoom = teleportRooms[index];
                    player.outputMessage(player.currentRoom.description());
                    player.rooms.Clear();
                }
            }
        }

        //Call back method for PlayerBuyAttempt
        public void playerBuyAttempt(Notification notification)
        {
            string itemName = (string)notification.userInfo["PlayerBuyAttempt"];
            Player player = (Player)notification.Object;
            if (notification.userInfo.ContainsKey("PlayerBuyAttempt"))
            {
                if(player.currentRoom == shopTrigger) //Ensures player can only purchase items if they are in the shop.
                {
                    if (salesList.TryGetValue(shopTrigger, out Item item)) //Checks if the item exists in the shop.
                    {
                        if (item.Type != ItemType.Key && itemName == item.Name)
                        {
                            if (player.Money >= item.BuyValue) //Checks if player has enough money to buy the item.
                            {
                                player.Money = player.Money - item.BuyValue;
                                shopTrigger.addItem(item);
                                player.outputMessage("\nYou hear a clatter as your purchased item falls to the ground.");
                            }
                            else
                            {
                                player.outputMessage("\nYou don't have enough money to buy that.");
                            }
                        }
                        else
                        {
                            player.outputMessage("\nThe shop isn't selling a " + itemName + ".");
                        }
                    }
                    else
                    {
                        player.outputMessage("\nThe shop isn't selling a " + itemName + ".");
                    }
                }
                else
                {
                    player.outputMessage("\nYou must be in a shop to buy items.");
                }
            }
        }

        //Call back method for PlayerSaleAttempt
        public void playerSellAttempt(Notification notification)
        {
            string itemName = (string)notification.userInfo["PlayerSellAttempt"];
            Player player = (Player)notification.Object;
            Dictionary<string, Item> inventory = new Dictionary<string, Item>();
            if (notification.userInfo.ContainsKey("PlayerSellAttempt"))
            {
                if (player.currentRoom == shopTrigger) //Makes sure player can only sell things in the shop.
                {
                    inventory = player.Inventory;
                    if (player.Inventory.Count != 0)
                    {
                        if (inventory.TryGetValue(itemName, out Item item))
                        {
                            if (item.Type != ItemType.Key) //Prevents player from selling key items.
                            {
                                //Gives player money based on the item's sale value. Removes item from the player's inventory and adjusts weight and volume.
                                player.Money = player.Money + item.SaleValue; 
                                player.outputMessage("\nYou exchange your " + item.Name + " for " +  item.SaleValue + " gold.");
                                inventory.Remove(itemName); 
                                player.WeightCapacity +=item.Weight;
                                player.VolumeCapacity += item.Volume;
                            }
                            else
                            {
                                player.outputMessage("\nYou can't sell that.");
                            }
                        }
                        else
                        {
                            player.outputMessage("\nThere is no " + itemName + " in your inventory.");
                        }
                    }
                    else
                    {
                        player.outputMessage("\nYour inventory is empty.");
                    }
                }
                else
                {
                    player.outputMessage("\nYou must be in a shop to sell your items.");
                }
            }
        }
    }
}

