using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class NullInventoryTooltip : ItemTooltip
    {
        public ITooltip Tooltip => null;
        public bool Show => false;

        public event UnityAction OnValueChanged = delegate { };
    }
}