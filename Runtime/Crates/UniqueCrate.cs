using System.Collections.Generic;

namespace Elysium.Items
{
    public class UniqueCrate : Crate
    {
        public UniqueCrate(List<IItemDropData> _possibilities) : base(_possibilities)
        {

        }

        public override IEnumerable<IItemStack> Open()
        {
            return RandomItemSelection.OpenUniqueCrate(Possibilities);
        }
    }
}
