using System.Collections;
using System.Collections.Generic;

namespace Elysium.Items
{
    public interface ICrate
    {
        Dictionary<IItem, float> Possibilities { get; }
        IEnumerable<IItemStack> Open();
    }
}
