using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public abstract class ConsumableSO : ItemSO
    {
        public override bool IsUsable => true;

        public override void Activate(ItemStack _stack, IInventoryUser _user)
        {
            if (ExecuteEffect(_user))
            {
                _user.Inventory.Remove(_stack.InventoryElement, 1, _stack);
            }
        }

        protected abstract bool ExecuteEffect(IInventoryUser _user);
    }
}
