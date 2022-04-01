using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface ITooltip
    {
        void Close();
        void Open(IItem _item, UnityAction _action);
    }
}