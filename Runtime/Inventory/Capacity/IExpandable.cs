using System.Collections.Generic;

namespace Elysium.Items
{
    public interface IExpandable
    {
        int AvailableExpansion { get; }
        int AvailableShrink { get; }

        bool Expand(int _quantity);
        bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems);
    }
}
