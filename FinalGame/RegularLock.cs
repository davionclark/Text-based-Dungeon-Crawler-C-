using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    public class RegularLock : Lockable
    {
        private Item key;
        private bool locked;
        public void Lock(Item idCard)
        {
            key = idCard;
            locked = true;
        }
        public void Unlock()
        {
            locked = false;
        }
        public bool IsLocked()
        {
            return locked;
        }
        public bool IsUnlocked()
        {
            return !locked;
        }
        public bool MayOpen()
        {
            return !locked;
        }
        public bool MayClose()
        {
            return true;
        }
    }
}