using System.Collections.Generic;

namespace Elysium.Items
{
    public class MultiCrate : Crate
    {
        public MultiCrate(List<IItemDropData> _possibilities) : base(_possibilities)
        {

        }

        public override IEnumerable<IItemStack> Open()
        {
            return RandomItemSelection.OpenMultiCrate(Possibilities);
        }
    }
}
