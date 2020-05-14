using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StarterGame
{
    public class Door
    {
        private Room roomA;
        private Room roomB;
        private bool closed;
        private Lockable theLock;
        private Item key;
        public bool Closed
        {
            get
            {
                return closed;
            }
            set
            {
                if (theLock != null)
                {
                    if (theLock.MayClose())
                    {
                        closed = value;
                    }
                }
                else
                {
                    closed = true;
                }
            }
        }
        public bool Open
        {
            get
            {
                return !closed;
            }
            set
            {
                if (theLock != null)
                {
                    if (theLock.MayOpen())
                    {
                        closed = !value;
                    }
                }
                else
                {
                    closed = !value;
                }
            }
        }
        public Door(Room roomA, Room roomB)
        {
            this.roomA = roomA;
            this.roomB = roomB;
            closed = false;
            theLock = new RegularLock();
        }

        public Room room(Room from)
        {
            if (roomA == from)
            {
                return roomB;
            }
            else
            {
                return roomA;
            }
        }
        public void Lock(Item idCard) //Locks the room with a specific key.
        {
            if (theLock != null)
            {
                key = idCard;
                theLock.Lock(idCard);
            }
        }
        public void Unlock()
        {
            if (theLock != null)
            {
                theLock.Unlock();
            }
        }
        public bool IsLocked()
        {
            if (theLock != null)
            {
                return theLock.IsLocked();
            }
            else
            {
                return false;
            }
        }
        public bool IsUnlocked()
        {
            if (theLock != null)
            {
                return theLock.IsUnlocked();
            }
            else
            {
                return true;
            }
        }
        public bool MayOpen()
        {
            if (theLock != null)
            {
                return theLock.MayOpen();
            }
            else
            {
                return true;
            }
        }
        public bool MayClose()
        {
            if (theLock != null)
            {
                return theLock.MayClose();
            }
            else
            {
                return true;
            }
        }

        public Item ReturnKey() //Returns the key that the door was initially locked with.
        {
            return key;
        }

        public static Door connect(Room room1, Room room2, string name1, string name2)
        {
            Door door = new Door(room1, room2);
            room1.setExit(name2, door);
            room2.setExit(name1, door);
            return door;
        }
    }
}
