using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;

namespace Elysium.Items
{
    public interface IItemStackCollection
    {
        IEnumerable<IItemStack> Stacks { get; }

        UnityEvent OnValueChanged { get; }

        bool Add(IItem _item, int _quantity);
        bool Remove(IItem _item, int _quantity);
        bool Contains(IItem _item);
        int Quantity(IItem _item);
        void Empty();
        void Swap(IItemStack _origin, IItemStack _destination);
    }
}
