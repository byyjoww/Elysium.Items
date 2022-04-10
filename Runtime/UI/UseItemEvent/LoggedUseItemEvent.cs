using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class LoggedUseItemEvent : DefaultUseItemEvent
    {
        public LoggedUseItemEvent(IItemUser _user) : base(_user)
        {

        }

        public override void Raise(IItemStack _stack, int _quantity)
        {
            Debug.Log($"[UseItemEvent] Stack containing item {_stack.Item.Name} was used {_quantity} time(s).");
            base.Raise(_stack, _quantity);
        }
    }
}
