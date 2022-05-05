using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Elysium.Items.Editor
{
    [CustomEditor(typeof(LimitedInventorySO))]
    public class LimitedInventorySOEditor : InventorySOEditorBase
    {
        private SerializedProperty capacity = default;


        protected override void OnEnable()
        {
            base.OnEnable();
            capacity = serializedObject.FindProperty("capacity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCapacity();
            DrawItems();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCapacity()
        {
            EditorGUILayout.PropertyField(capacity);
        }
    }
}
