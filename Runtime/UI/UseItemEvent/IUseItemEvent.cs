using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IUseItemEvent
    {
        event UnityAction<IItemStack, int> OnRaise;
        void Raise(IItemStack _stack, int _quantity);
    }
}
