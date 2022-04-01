using UnityEngine;
using Elysium.Core.Utils;
using Elysium.Core.Utils.Filters;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "InventoryFilterConfigSO", menuName = "Scriptable Objects/Inventory/Filter Config")]
    public class InventoryFilterConfigSO : FilterConfigSO<IItem, IItemFilter>, IItemFilterConfig { }
    public interface IItemFilterConfig : IFilterConfig<IItem, IItemFilter> { }
    public class NullItemFilter : NullFilter<IItem>, IItemFilter { }

#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryFilterConfigSO))]
    public class InventoryFilterConfigSOEditor : FilterConfigSOEditor<IItemFilter>
    {

    }
#endif
}
