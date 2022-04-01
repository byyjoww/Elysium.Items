using UnityEngine;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemDropData : IItemDropData
    {
        [SerializeField] private IItem item = default;
        [SerializeField] private float chance = 0f;

        public IItem Item => item;
        public float Chance => chance;

        public ItemDropData()
        {

        }

        public ItemDropData(IItem _item, float _chance)
        {
            this.item = _item;
            this.chance = _chance;
        }
    }
}
