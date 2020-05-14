using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace StarterGame
{
    public class Player
    {
        public bool inBattle; 
        public List<Room> rooms = new List<Room>();
        Dictionary<string, Item> inventory = new Dictionary<string, Item>();
        Dictionary<string, object> userInfo = new Dictionary<string, object>(); //Dictionary to carry information from notifications
        private bool isTeleporting;
        private Weapon startingWeapon = new Weapon("fists", 0, false, 1, 1, 0, 0, 0, -1); //Default weapon used when nothing is equipped.
        private Weapon equippedWeapon;
        private int minimumHp = 0; //Value the player's health has to fall to in order to die.
        private int maximumHp = 10; //Maximum player health
        private Room _currentRoom;
        private int weightCapacity = 10; //How much weight the player can carry
        private float volumeCapacity = 4f; //Maximum player volume;
        private int money = 0; //Starting player currency

        public int MaximumHp
        {
            get { return maximumHp; }
            set { maximumHp = value; }
        }

        public int MinimumHp
        {
            get { return minimumHp; }
            set { minimumHp = value; }
        }
        public Item StartingWeapon
        {
            get { return startingWeapon; }
            set { startingWeapon = (Weapon) value; }
        }

        private int hp;
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
        public Dictionary<string, Item> Inventory
        {
            get { return inventory; }
        }
        public bool IsTeleporting
        {
            get { return isTeleporting; }
            set { isTeleporting = value; }
        }

        public int WeightCapacity
        {
            get { return weightCapacity; }
            set { weightCapacity = value; }
        }
        public float VolumeCapacity
        {
            get { return volumeCapacity; }
            set { volumeCapacity = value; }
        }
        public bool InBattle
        {
            set { inBattle = value; }
        }
        public int Money
        {
            get { return money; }
            set { money = value; }
        }
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

        public Item EquippedWeapon
        {
            get { return equippedWeapon; }
            set { equippedWeapon = (Weapon)value; }
        }
        public Player(Room room)//GameOutput output, sets current player HP to the maximum value. Sets starting weapon.
        {
            this.EquippedWeapon = this.StartingWeapon;
            this.Hp = this.MaximumHp;
            _currentRoom = room;
        }

        public void walkTo(string direction) //Walking command. Initiated from GoCommand.
        {
            if (inBattle == false) //Ensures player isn't in battle.
            {
                if (!userInfo.ContainsKey("PlayerEnteredRoom")) //Notification dictionary checks.
                {
                    userInfo.Add("PlayerEnteredRoom", _currentRoom);
                }
                else
                {
                    userInfo["PlayerEnteredRoom"] = _currentRoom;
                }
                NotificationCenter.Instance.postNotification(new Notification("PlayerEnteredRoom", this, userInfo)); //Sends player's position before they enter a new room. 
                Door door = this._currentRoom.getExit(direction);
                if (door != null && this._currentRoom.roomID != 99) //RoomID 99 is the id number of the teleport room. Ensure that it is always the teleport room's id.
                {
                    if (door.Open)
                    {
                        if (door.IsUnlocked())
                        {
                            rooms.Add(_currentRoom);
                            this._currentRoom = door.room(this._currentRoom);
                            if (!userInfo.ContainsKey("PlayersCurrentRoom"))
                            {
                                userInfo.Add("PlayersCurrentRoom", _currentRoom);
                            }
                            else
                            {
                                userInfo["PlayersCurrentRoom"] = _currentRoom;
                            }
                            this.outputMessage("\n" + this._currentRoom.description());
                            NotificationCenter.Instance.postNotification(new Notification("PlayersCurrentRoom", this, userInfo)); //Sends player's new position through a notification.
                        }
                        else
                        {
                            this.outputMessage("\nThe " + direction + " door is closed.");
                        }
                    }
                    else
                    {
                        this.outputMessage("\nThe " + direction + " door is closed.");
                    }
                }
                else if (isTeleporting == false)
                {
                    this.outputMessage("\nThere is no exit to the " + direction + ".");
                }
                isTeleporting = false; //Notification sends current world. Game world checks if the room is the teleportation world. This ensures that players stop teleporting.
            }
            else
            {
                this.outputMessage("\nYou're in a battle, you can't do that!");
            }
        }

        //Takes the player back to the last room they visited. 
        public void back()
        {
            if (this.currentRoom.roomID == 99) //Value must equal teleportation room's ID. Prevents player form using the back command while in the telportation room.
            {
                this.outputMessage("\nAn invisible force is stopping you.");
            }
            else if (rooms.Count != 0 && this.inBattle == false) //Prevents player from using the back command if they cannot go back farther or if they are in battle.
            {
                this._currentRoom = rooms.Last();
                this.outputMessage("\n" + this._currentRoom.description());
                rooms.RemoveAt(rooms.Count - 1);
            }
            else
            {
                this.outputMessage("\nYou cannot go back any farther.");
            }
        }

        public void buy(string itemName) //Sends out a notification whenever a player attempts to buy an item
        {
            if (!userInfo.ContainsKey("PlayerBuyAttempt"))
            {
                userInfo.Add("PlayerBuyAttempt", itemName);
            }
            else
            {
                userInfo["PlayerBuyAttempt"] = itemName;
            }
            NotificationCenter.Instance.postNotification(new Notification("PlayerBuyAttempt", this, userInfo));
        }

        public void sell(string itemName) //Sends out a notification whenever a player attempts to sell an item.
        {
            if (!userInfo.ContainsKey("PlayerSellAttempt"))
            {
                userInfo.Add("PlayerSellAttempt", itemName);
            }
            else
            {
                userInfo["PlayerSellAttempt"] = itemName;
            }
            NotificationCenter.Instance.postNotification(new Notification("PlayerSellAttempt", this, userInfo));
        }

        public void grab(string itemName) //Allows player to pick up item objects. Initiated from GrabCommand.
        {

            Item item = _currentRoom.getItem(itemName);
            if (item == null)
            {
                this.outputMessage("\nThere is no " + itemName + " in this room that can be grabbed.");
            }
            else if (inventory.ContainsKey(itemName)) //Checks if item already exists in inventory
            {
                this.outputMessage("\nYou already have one.");
            }
            else if(!item.CanBePickedUp) //Checks if the items property allows it to be picked up or not.
            {
                this.outputMessage("\nThe " + itemName + " cannot be grabbed.");
            }
            else if (volumeCapacity - item.Volume < 0) //Ensures volume hasn't been exceeded.
            {
                this.outputMessage("\nThat item is too large to fit in your inventory. Drop an item and try again.");
            }
            else if (weightCapacity - item.Weight < 0) //Checks if player will exceed weight limit by picking up item.
            {
                this.outputMessage("\nYou're carrying too much weight. Drop an item first!");
            }
            else
            {
                //Increments player weight when picking up item. Removes item from the room and sends a notification that removes the item description as well. Adds item to inventory.
                weightCapacity -= item.Weight;
                volumeCapacity -= item.Volume;
                userInfo.Add("PlayerPickedUpItem", item);
                NotificationCenter.Instance.postNotification(new Notification("PlayerPickedUpItem", this, userInfo));
                currentRoom.removeItem(item);
                inventory.Add(itemName, item);
                this.outputMessage("\nYou grabbed the " + itemName + ".");
            }
        }

        public void drop(string itemName) //Allows players to drop items. Initiated from DropCommand.
        {
            Item item = null;
            if(inventory.Count == 0)
            {
                this.outputMessage("\nYou do not have any items in your inventory!");
            }
            else if (inventory.TryGetValue(itemName, out item))
            {
                //Removes item from inventory and adds item and item description to the room. Decreases player weight. Frees up inventory volume. Sends notification that the player dropped an item.
                userInfo.Add("PlayerDroppedItem", item);
                NotificationCenter.Instance.postNotification(new Notification("PlayerDroppedItem", this, userInfo));
                inventory.Remove(itemName);
                _currentRoom.addItem(item);
                weightCapacity += item.Weight;
                volumeCapacity += item.Volume;
                this.outputMessage("\nYou dropped the " + itemName + ".");
            }
            else
            {
                this.outputMessage("\nYou do not have a " + itemName + " in your inventory.");
            }
        }

        public void outputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void inventoryList() //Prints out the items in the player's inventory.
        {
            this.outputMessage("\nWeapon: " + equippedWeapon.Name + "\n\nYou have " + this.Money + " coins.");
            if (inventory.Count != 0)
            {
                string output = "\nYou possess the following items: ";
                foreach (var item in inventory)
                {
                    output = output + "|" + item.Key + "| ";
                }
                this.outputMessage(output);
            }
            else
            {
                this.outputMessage("\nYour inventory is empty.");
            }
        }

        public void open(string direction, string doorName) //Allows players to open doors. Initiated from OpenCommand
        {
            Door door = this._currentRoom.getExit(direction);
            if (doorName == "door")
            {
                if (door != null)
                {
                    if (door.MayOpen() && door.Open == false) //If door may be opened and is not already opened.
                    {
                        door.Open = true;
                        this.outputMessage("\nThe door to the " + direction + " is now open.");
                    }
                    else if (door.Open)
                    {
                        this.outputMessage("\nThe door to the " + direction + " is already open.");
                    }
                    else
                    {
                        if (door.IsLocked())
                        {
                            this.outputMessage("\nThe door to the " + direction + " is locked.");
                        }
                    }
                }
                else
                {
                    this.outputMessage("\nThere is no door to the " + direction + ".");
                }
            }
            else
            {
                this.outputMessage("\nYou can't open the " + doorName + ".");
            }
        }
        public void unlock(string direction, string doorName) //Allows player to unlock doors. Initiated from UnlockCommand.
        {
            Door door = this._currentRoom.getExit(direction);
            if (doorName == "door")
            {
                if (door != null)
                {
                    if (door.IsLocked())
                    {
                        if (inventory.ContainsValue(door.ReturnKey())) //Doors are initialized with a specific key during creation. This checks if that key is in your inventory.
                        {
                            door.Unlock();
                            this.outputMessage("\nYou unlocked the " + doorName + ".");
                        }
                        else
                        {
                            this.outputMessage("\nYou don't have the necessary key to open that door.");
                        }
                    }
                    else
                    {
                        this.outputMessage("\nThe door is already unlocked.");
                    }
                }
                else
                {
                    this.outputMessage("\nThere is no door to the " + direction + ".");
                }
            }
            else
            {
                this.outputMessage("\nYou can't unlock the " + doorName + ".");
            }
        }

        public void equip(string itemName) //Allows players to equip items. Initiated from EquipCommand.
        {
            if (inventory.Count != 0) //Makes sure inventory isn't empty.
            {
                if (inventory.TryGetValue(itemName, out Item item)) //Checks if the item the player wants to equip is in their inventory.
                {
                    if (item.Type == ItemType.Weapon) //Makes sure attempted item to equip is a weapon, not a key. Currently no armor is implemented.
                    {
                        if (equippedWeapon == null || equippedWeapon == startingWeapon) //If player's hand is null or has the starting weapon in it, then equip the item WITHOUT adding the previously equipped item to the inventory.
                        {
                            this.EquippedWeapon = (Weapon)item;
                            this.EquippedWeapon.Name = item.Name;
                            inventory.Remove(itemName);
                            volumeCapacity += item.Volume;
                            this.outputMessage("\nYou equip the " + itemName + ".");
                        }
                        else
                        {
                            this.unequip(equippedWeapon.Name); //Unequips the current weapon.
                            this.equip(equippedWeapon.Name);
                        }
                    }
                    else
                    {
                        this.outputMessage("\nYou cannot equip that.");
                    }
                }
                else
                {
                    this.outputMessage("\nThere is no " + itemName + " in your inventory.");
                }
            }
            else
            {
                this.outputMessage("\nThe item must be in your inventory to equip it.");
            }
        }

        public void unequip(string itemName) //Allows players to unequip items. Initiated from UnequipCommand
        {
            if (equippedWeapon != null && equippedWeapon != startingWeapon) //Prevents player from unequipping starting weapon or using the command if their equipped weapon is null.
            {
                if (equippedWeapon.Name == itemName)
                {
                    if (!(volumeCapacity - equippedWeapon.Volume <= 0))
                    {
                        //Adjusts inventory volume and removes item from the equipped slot and places it in your inventory.
                        volumeCapacity -= equippedWeapon.Volume;
                        inventory.Add(itemName, equippedWeapon);
                        removeWeapon();
                        this.outputMessage("\nYou store the " + itemName + " in your inventory.");
                    }
                    else
                    {
                        this.outputMessage("\nYou cannot fit the " + this.EquippedWeapon.Name + " in your inventory. Drop an item and try again.");
                    }
                }
                else
                {
                    this.outputMessage("\nYou do not have a " + itemName + " equipped.");
                }
            }
            else
            {
                this.outputMessage("\nYou do not have any items equipped.");
            }
        }
        public void removeWeapon() //Whenever a weapon is unequipped it defaults it to the starting weapon.
        {
            this.EquippedWeapon = startingWeapon;
        }
    }
}
