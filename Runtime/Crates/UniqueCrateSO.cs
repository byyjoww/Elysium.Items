using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "UniqueCrateSO", menuName = "Scriptable Objects/Items/Crates/Unique")]
    public class UniqueCrateSO : CrateSO
    {
        public override IEnumerable<IItemStack> Open()
        {
            return RandomItemSelection.OpenUniqueCrate(Possibilities);
        }
    }
}
