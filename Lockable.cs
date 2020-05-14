using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
    public interface Lockable
    {
        void Lock(Item item);
        void Unlock();
        bool IsLocked();
        bool IsUnlocked();
        bool MayOpen();
        bool MayClose();
    }
}

