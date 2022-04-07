using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IInventoryTooltip
    {        
        ITooltip Tooltip { get; }
        bool Show { get; }

        event UnityAction OnValueChanged;
    }
}