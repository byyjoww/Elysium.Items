using Elysium.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class VisualInventorySlotPoolView : PoolView<VisualInventorySlot>, IInventorySlotView
    {
        public new IEnumerable<IVisualInventorySlot> Set(int _numOfSlots)
        {
            return base.Set(_numOfSlots).Cast<IVisualInventorySlot>();
        }
    }
}