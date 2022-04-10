using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class NullUseItemEvent : IUseItemEvent
    {
        public event UnityAction<IItemStack, int> OnRaise = delegate { };

        public void Raise(IItemStack _stack, int _quantity)
        {
            
        }
    }
}
