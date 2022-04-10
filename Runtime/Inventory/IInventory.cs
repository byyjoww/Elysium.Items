using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Items
{
    public interface IInventory : IEnumerable<IItemStack>
    {
        IItemStackCollection Items { get; }

        event UnityAction OnValueChanged;

        bool Add(IItem _item, int _quantity);
        bool Remove(IItem _item, int _quantity);
        int Quantity(IItem _item);
        bool Contains(IItem _item);
        void Empty();
    }
}
