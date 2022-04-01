using System.Collections.Generic;
using System.Linq;

namespace Elysium.Items
{
    public abstract class Crate : ICrate
    {
        private List<IItemDropData> possibilities = new List<IItemDropData>();        

        public Dictionary<IItem, float> Possibilities => possibilities.ToDictionary(x => x.Item, x => x.Chance);

        protected Crate(List<IItemDropData> _possibilities)
        {
            this.possibilities = _possibilities;
        }

        public abstract IEnumerable<IItemStack> Open();
    }
}
