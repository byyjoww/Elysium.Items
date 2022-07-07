using Elysium.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class InventoryViewSlotPoolSpawner : PoolSpawner<InventoryViewSlot>, IInventoryViewSlotSpawner
    {
        public new IEnumerable<IInventoryViewSlot> Set(int _numOfSlots)
        {
            return base.Set(_numOfSlots).Cast<IInventoryViewSlot>();
        }
    }
}