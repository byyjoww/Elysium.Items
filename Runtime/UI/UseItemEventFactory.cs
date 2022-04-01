using Elysium.Core;

namespace Elysium.Items.UI
{
    public class UseItemEventFactory
    {
        private IUseItemEvent useItemEvent = default;
        private ITooltip tooltip = default;

        public UseItemEventFactory(IItemStack _stack, IUseItemEvent _event, bool _forceTrigger, ITooltip _tooltip, bool _showTooltip)
        {
            this.tooltip = _showTooltip ? _tooltip : null;
            this.useItemEvent = (_forceTrigger || _stack.Item.IsUsable) ? _event : null;
        }

        public IUseItemEvent Create()
        {
            if (tooltip is null && useItemEvent is null) { return null; }
            return tooltip is null ? useItemEvent : new UseItemEventTooltipDecorator(useItemEvent, tooltip);
        }
    }
}