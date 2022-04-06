using Elysium.Core.Utils.Filters;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IInventoryFilterer
    {
        event UnityAction OnValueChanged;

        void Init(IItemFilterConfig _config);
        bool Evaluate(IItem _item);
        void End();
    }
}