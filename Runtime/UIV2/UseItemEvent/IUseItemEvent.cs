using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IUseItemEvent
    {
        event UnityAction<IInventory, IItemStack> OnRaise;
        void Raise(IInventory _inventory, IItemStack _stack);
    }
}
