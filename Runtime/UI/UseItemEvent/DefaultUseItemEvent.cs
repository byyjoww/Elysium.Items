using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class DefaultUseItemEvent : IUseItemEvent
    {
        private IItemUser user = default;

        public DefaultUseItemEvent(IItemUser _user)
        {
            this.user = _user;
        }

        public event UnityAction<IItemStack, int> OnRaise = delegate { };

        public virtual void Raise(IItemStack _stack, int _quantity)
        {
            OnRaise?.Invoke(_stack, _quantity);
            bool success = _stack.Use(user, _quantity);
        }
    }
}
