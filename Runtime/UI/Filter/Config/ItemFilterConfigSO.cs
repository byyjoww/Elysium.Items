using UnityEngine;
using Elysium.Core.Utils;
using Elysium.Core.Utils.Filters;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "ItemFilterConfigSO", menuName = "Scriptable Objects/Items/Filter Config")]
    public class ItemFilterConfigSO : FilterConfigSO<IItem, IItemFilter>, IItemFilterConfig 
    { 

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemFilterConfigSO))]
    public class ItemFilterConfigSOEditor : FilterConfigSOEditor<IItemFilter>
    {

    }
#endif
}
