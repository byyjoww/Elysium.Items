using Elysium.Core;
using Elysium.Core.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class VisualInventorySlotWithTooltip : VisualInventorySlot
    {
        [Header("Tooltip")]
        [SerializeField, RequireInterface(typeof(ITooltip))] 
        protected UnityEngine.Object tooltip = default;
        [SerializeField] private BoolValueSO show = default;

        private ITooltip Tooltip => tooltip as ITooltip;

        protected override void OnClick()
        {
            UnityAction openTooltip = delegate { Tooltip.Open(stack.Item, base.OnClick); };
            UnityAction onClick = show.Value ? openTooltip : base.OnClick;
            onClick?.Invoke();
        }
    }
}