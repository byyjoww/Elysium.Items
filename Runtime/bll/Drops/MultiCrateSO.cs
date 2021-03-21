using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "MultiCrateSO", menuName = "Scriptable Objects/Items/Crates/Multi")]
    public class MultiCrateSO : UniqueCrateSO
    {
        public override List<ItemStack> Open() => RandomItemSelection.OpenMultiChest(PossibleItemsDictionary);
    }
}