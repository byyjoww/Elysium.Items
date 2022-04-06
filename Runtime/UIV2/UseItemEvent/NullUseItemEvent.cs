using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class NullUseItemEvent : IUseItemEvent
    {
        public event UnityAction<IInventory, IItemStack> OnRaise = delegate { };

        public void Raise(IInventory _inventory, IItemStack _stack)
        {
            
        }
    }
}
