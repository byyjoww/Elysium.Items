using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public interface IInventoryUser
    {
        InventorySO Inventory { get; }
        GameObject Object { get; }

        // Add a list of functions that items can perform here
        void Heal(int _amount);
    }
}