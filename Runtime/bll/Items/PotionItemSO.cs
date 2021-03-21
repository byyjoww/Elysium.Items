using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "PotionItemSO", menuName = "Scriptable Objects/Items/Potion")]
    public class PotionItemSO : ConsumableSO
    {
        [SerializeField] private int healthRecovery = default;

        protected override bool ExecuteEffect(IInventoryUser _user)
        {
            _user.Heal(healthRecovery);
            return true;
        }
    }
}