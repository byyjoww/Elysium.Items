using System.Collections.Generic;

namespace Elysium.Items.UI
{
    public interface IInventoryViewSlotSpawner
    {
        IEnumerable<IInventoryViewSlot> Set(int _numOfSlots);
    }
}