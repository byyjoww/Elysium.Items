namespace Elysium.Items.Editor
{
    public class UnlimitedInventoryEditorBase : InventoryEditorBase
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawItems();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
