using UnityEditor;

namespace Elysium.Items.Editor
{
    public class LimitedInventoryEditorBase : InventoryEditorBase
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
