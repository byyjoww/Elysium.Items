using Elysium.Core.Utils.Filters;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public interface IItemFilterer
    {
        event UnityAction OnValueChanged;

        void Init(IItemFilterConfig _config);
        bool Evaluate(IItem _item);
        void End();
    }
}