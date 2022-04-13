using Elysium.Items.UI;
using UnityEngine;

namespace Elysium.Items.Samples
{
    public class SampleConsumableItemFilter : IncludeTypeFilter<SampleConsumableItem>
    {
        [SerializeField] private string name = "Consumables";

        public override string Name => name;
    }

    public class SampleNonConsumableItemFilter : ExcludeTypeFilter<SampleConsumableItem>
    {
        [SerializeField] private string name = "Non-Consumables";

        public override string Name => name;
    }
}

