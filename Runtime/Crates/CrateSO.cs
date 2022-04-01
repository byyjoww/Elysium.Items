using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    public abstract class CrateSO : ScriptableObject, ICrate
    {
        [SerializeField] private List<ItemDropData> possibleItemsList = new List<ItemDropData>();
        public Dictionary<IItem, float> Possibilities => possibleItemsList.ToDictionary(x => x.Item, x => x.Chance);

        public abstract IEnumerable<IItemStack> Open();
    }
}
