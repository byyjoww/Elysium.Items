using UnityEngine;
using Elysium.Core.Utils;
using Elysium.Core.Utils.Filters;
using System.Collections.Generic;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "InventoryFilterConfigSO", menuName = "Scriptable Objects/Inventory/Filter Config")]
    public class InventoryFilterConfigSO : FilterConfigSO<IItem, IItemFilter>, IItemFilterConfig { }

    public interface IItemFilterConfig : IFilterConfig<IItem, IItemFilter> { }

    public class NullItemFilterConfig : IItemFilterConfig
    {
        public List<IItemFilter> Filters => throw new System.NotImplementedException();
        public IFilter<IItem> Default => throw new System.NotImplementedException();

        public bool Visible => throw new System.NotImplementedException();

        public event UnityAction OnValueChanged;

        public void Add(IItemFilter _filter, bool _acceptDuplicate = false)
        {
            
        }

        public void Remove(IItemFilter _filter)
        {
            
        }
    }

    public class NullItemFilter : NullFilter<IItem>, IItemFilter { }

#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryFilterConfigSO))]
    public class InventoryFilterConfigSOEditor : FilterConfigSOEditor<IItemFilter>
    {

    }
#endif
}
