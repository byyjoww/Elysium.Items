using Elysium.Core.Utils;
using Elysium.Core.Utils.Filters;
using Elysium.Core.Utils.Tools;
using UnityEngine;

namespace Elysium.Items.UI
{
    public interface IItemFilter : IFilter<IItem>
    {

    }

    public class ConsumableItemFilter : IncludeTypeFilter<GenericConsumableItem>
    {
        [SerializeField] private string name = "Consumables";

        public override string Name => name;
    }

    public class NonConsumableItemFilter : ExcludeTypeFilter<GenericConsumableItem>
    {
        [SerializeField] private string name = "Non-Consumables";

        public override string Name => name;
    }
}
