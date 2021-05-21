using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.Core;
using Elysium.Utils.Attributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "ItemStackEventSO", menuName = "Scriptable Objects/Scriptable Events/Custom/Item Stack Event", order = 1)]
    public class ItemStackEventSO : GenericEventSO<ItemStack>
    {
#if UNITY_EDITOR

        [Header("Editor Only")]
        [SerializeField] private ItemStack editorData;

        [CustomEditor(typeof(ItemStackEventSO), true)]
        public class ItemStackEventSOEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                if (GUILayout.Button("Raise"))
                {
                    var e = target as ItemStackEventSO;
                    e.Raise(e.editorData);
                }
            }
        }
#endif
    }
}
