using UnityEngine;

namespace Elysium.Items.UI
{
    public class MiscFilter : IItemFilter
    {
        [SerializeField] private string name = "Misc";

        public string Name => name;

        public bool Evaluate(IItem _element)
        {
            return !_element.IsUsable;
        }
    }
}
