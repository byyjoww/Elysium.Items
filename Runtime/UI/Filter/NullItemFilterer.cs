using Elysium.Core.Utils.Filters;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class NullItemFilterer : IItemFilterer
    {
        public event UnityAction OnValueChanged = delegate { };

        public void Init(IItemFilterConfig _config)
        {

        }

        public bool Evaluate(IItem _item)
        {
            return true;
        }

        public void End()
        {
            
        }
    }
}