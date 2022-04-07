using Elysium.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "UseItemEventSO", menuName = "Scriptable Objects/Events/Use Item", order = 1)]
    public class UseItemEventSO : GenericEventSO<IInventory, IItemStack>, IUseItemEvent
    {
#if UNITY_EDITOR

        [Header("Editor Only")]
        [SerializeField] private IInventory editorData1;
        [SerializeField] private IItemStack editorData2;

        [CustomEditor(typeof(UseItemEventSO), true)]
        public class UseItemEventSOEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                if (GUILayout.Button("Raise"))
                {
                    var e = target as UseItemEventSO;
                    e.Raise(e.editorData1, e.editorData2);
                }
            }
        }
#endif
    }
}
