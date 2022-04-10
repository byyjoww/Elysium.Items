using Elysium.Core;
using UnityEngine;
using System.Linq;
using Elysium.Core.Attributes;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "UseItemEventSO", menuName = "Scriptable Objects/Events/Use Item", order = 1)]
    public class UseItemEventSO : GenericEventSO<IItemStack, int>, IUseItemEvent
    {
#if UNITY_EDITOR
        [Header("Editor Only")]
        [RequireInterface(typeof(IInventory))]
        [SerializeField] private UnityEngine.Object inventory;
        [SerializeField] private string itemID;
        [SerializeField] private int quantity;

        private IInventory Inventory => inventory as IInventory;

        [CustomEditor(typeof(UseItemEventSO), true)]
        public class UseItemEventSOEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                if (GUILayout.Button("Raise"))
                {
                    var e = target as UseItemEventSO;
                    e.Raise(e.Inventory.FirstOrDefault(x => x.Item.ItemID == Guid.Parse(e.itemID)), e.quantity);
                }
            }
        }
#endif
    }
}
