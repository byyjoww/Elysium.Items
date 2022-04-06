using Elysium.Core;
using Elysium.Core.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class InventoryTooltip : IInventoryTooltip
    {
        [SerializeField] private bool enabled = false;

        [RequireInterface(typeof(ITooltip))]
        [SerializeField, ConditionalField(nameof(enabled))] private UnityEngine.Object tooltip = default;
        [SerializeField, ConditionalField(nameof(enabled))] private BoolValueSO showTooltip = default;

        public bool Show
        {
            get
            {
                return enabled ? showTooltip.Value : false;
            }
        }

        public ITooltip Tooltip => tooltip as ITooltip;

        public event UnityAction OnValueChanged = delegate { };

        public void Init()
        {
            if (!enabled) { return; }
            showTooltip.OnValueChanged += TriggerOnValueChanged;
        }

        public void End()
        {
            if (!enabled) { return; }
            showTooltip.OnValueChanged -= TriggerOnValueChanged;
        }

        private void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }
    }
}
