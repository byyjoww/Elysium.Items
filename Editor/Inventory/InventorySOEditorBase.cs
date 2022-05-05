using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Elysium.Items.Editor
{
    public class InventorySOEditorBase : UnityEditor.Editor
    {
        protected IInventory inventory = default;

        protected virtual GUIStyle WindowStyle
        {
            get
            {
                GUIStyle style = GUI.skin.window;
                style.padding.top = 5;
                style.padding.bottom = 5;
                style.padding.left = 10;
                return style;
            }
        }

        protected virtual void OnEnable()
        {
            inventory = (IInventory)target;
        }

        protected virtual void DrawItems()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Items", EditorStyles.label);
            EditorGUILayout.BeginVertical(WindowStyle);
            int numOfItems = inventory.Items.Count();
            for (int i = 0; i < numOfItems; i++)
            {
                EditorGUILayout.BeginHorizontal();

                IItemStack stack = inventory.Items.ElementAt(i);

                string itemID = stack.Item.ItemID.ToString();
                GUILayout.Label("ID:", GUILayout.Width(20));
                GUILayout.TextField(itemID);
                GUILayout.Space(10);
                string itemName = stack.Item.Name;
                GUILayout.Label("Name:", GUILayout.Width(40));
                GUILayout.TextField(itemName);
                GUILayout.Space(10);
                int itemQuantity = stack.Quantity;
                GUILayout.Label("Quantity:", GUILayout.Width(55));
                GUILayout.TextField(itemQuantity.ToString());
                GUILayout.Space(10);
                int maxStack = stack.Item.MaxStack;
                GUILayout.Label("Max Stack:", GUILayout.Width(70));
                GUILayout.TextField(maxStack.ToString());

                EditorGUILayout.EndHorizontal();
            }

            if (numOfItems == 0)
            {
                GUILayout.Label("Empty");
            }

            EditorGUILayout.EndVertical();
        }        
    }
}