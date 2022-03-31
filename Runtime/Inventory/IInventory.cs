using System;
using UnityEngine.Events;

namespace Elysium.Items
{
    public interface IInventory
    {
        IItemStackCollection Items { get; }

        event UnityAction OnValueChanged;

        bool Add(IItem _item, int _quantity);
        bool Remove(IItem _item, int _quantity);
        int Quantity(IItem _item);
        bool Contains(IItem _item);
        void Swap(IItemStack _origin, IItemStack _destination);
        void Empty();
    }
}
