using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IInventoryViewSlot
    {
        void Setup(IInventoryViewSlotConfig _config);
        void Swap(IItemStack _stack);
    }
}