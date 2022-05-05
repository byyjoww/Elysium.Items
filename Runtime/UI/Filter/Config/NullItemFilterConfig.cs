using Elysium.Core.Utils.Filters;
using System.Collections.Generic;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace Elysium.Items.UI
{
    public class NullItemFilterConfig : IItemFilterConfig
    {
        public List<IItemFilter> Filters => throw new System.NotImplementedException();
        public IFilter<IItem> Default => throw new System.NotImplementedException();

        public bool Visible => throw new System.NotImplementedException();

        public event UnityAction OnValueChanged = delegate { };

        public void Add(IItemFilter _filter, bool _acceptDuplicate = false)
        {
            
        }

        public void Remove(IItemFilter _filter)
        {
            
        }
    }
}
