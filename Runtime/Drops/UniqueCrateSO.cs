using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "UniqueCrateSO", menuName = "Scriptable Objects/Item/Crates/Unique")]
    public class UniqueCrateSO : ScriptableObject
    {
        [SerializeField] private List<ItemDropData> possibleItemsList = new List<ItemDropData>();
        public Dictionary<IInventoryElement, float> PossibleItemsDictionary => possibleItemsList.ToDictionary(x => x.Item, x => x.Chance);

        public virtual List<ItemStack> Open() => RandomItemSelection.OpenUniqueChest(PossibleItemsDictionary);
    }
}