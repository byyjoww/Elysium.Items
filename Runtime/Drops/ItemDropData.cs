using Elysium.Utils.Attributes;
using UnityEngine;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemDropData
    {
        [RequireInterface(typeof(IInventoryElement))] [SerializeField] private ScriptableObject item = default;
        [Range(0, 100)] [SerializeField] private float chance = 0f;

        public IInventoryElement Item => item as IInventoryElement;
        public float Chance => chance;
    }
}