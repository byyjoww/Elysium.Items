using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Elysium.Items.Editor
{
    [CustomEditor(typeof(UnlimitedInventorySO))]
    public class UnlimitedInventorySOEditor : InventorySOEditorBase
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawItems();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
