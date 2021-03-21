using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "MiscItemSO", menuName = "Scriptable Objects/Items/Misc")]
    public class MiscItemSO : ItemSO
    {
        public override bool IsUsable => false;
    }
}
