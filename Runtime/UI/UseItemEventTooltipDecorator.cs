using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class UseItemEventTooltipDecorator : IUseItemEvent
    {
        private IUseItemEvent useItemEvent = default;
        private ITooltip tooltip = default;

        public event UnityAction<IInventory, IItemStack> OnRaise = delegate { };

        public UseItemEventTooltipDecorator(IUseItemEvent _event, ITooltip _tooltip)
        {
            this.useItemEvent = _event;
            this.tooltip = _tooltip;
            useItemEvent.OnRaise += TriggerOnRaise;
        }

        public void Raise(IInventory _inventory, IItemStack _stack)
        {
            tooltip.Open(_stack.Item, () => TriggerOnRaise(_inventory, _stack));
        }

        public void TriggerOnRaise(IInventory _inventory, IItemStack _stack)
        {
            OnRaise?.Invoke(_inventory, _stack);
        }
    }
}