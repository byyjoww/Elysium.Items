using System.Collections.Generic;

namespace Elysium.Items.UI
{
    public interface IInventorySlotView
    {
        IEnumerable<IVisualInventorySlot> Set(int _numOfSlots);
    }
}