using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;

namespace Elysium.Items
{
    public interface IItemStackCollection
    {
        IEnumerable<IItemStack> Stacks { get; }

        event UnityAction OnItemsChanged;
        event UnityAction OnValueChanged;

        bool Add(IItem _item, int _quantity);
        bool Remove(IItem _item, int _quantity);
        bool Contains(IItem _item);
        int Quantity(IItem _item);
        void Empty();
    }
}
