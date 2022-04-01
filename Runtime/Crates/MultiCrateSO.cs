using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "MultiCrateSO", menuName = "Scriptable Objects/Items/Crates/Multi")]
    public class MultiCrateSO : CrateSO
    {
        public override IEnumerable<IItemStack> Open()
        {
            return RandomItemSelection.OpenMultiCrate(Possibilities);
        }
    }
}
