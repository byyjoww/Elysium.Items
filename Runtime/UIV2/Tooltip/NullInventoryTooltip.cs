using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class NullInventoryTooltip : IInventoryTooltip
    {
        public ITooltip Tooltip => null;
        public bool Show => false;

        public event UnityAction OnValueChanged = delegate { };
    }
}