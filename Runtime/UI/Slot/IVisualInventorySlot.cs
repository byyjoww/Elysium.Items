using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IVisualInventorySlot
    {
        void Setup(VisualInventorySlotConfig _config);
        void Swap(IItemStack _stack);
    }
}