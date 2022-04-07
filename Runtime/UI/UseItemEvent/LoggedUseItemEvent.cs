using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class LoggedUseItemEvent : IUseItemEvent
    {
        public event UnityAction<IInventory, IItemStack> OnRaise = delegate { };

        public void Raise(IInventory _inventory, IItemStack _stack)
        {
            Debug.Log($"Stack containing item {_stack.Item.Name} was used");
        }
    }
}
