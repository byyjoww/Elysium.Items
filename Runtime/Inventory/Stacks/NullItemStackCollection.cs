using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class NullItemStackCollection : IItemStackCollection
    {
        public IEnumerable<IItemStack> Stacks => new List<IItemStack>();
        public UnityEvent OnValueChanged { get; private set; } = new UnityEvent();

        public bool Add(IItem _item, int _quantity)
        {
            return false;
        }

        public bool Contains(IItem _item)
        {
            return false;
        }

        public void Empty()
        {
            
        }

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Stacks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Quantity(IItem _item)
        {
            return 0;
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return false;
        }
    }
}