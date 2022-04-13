using UnityEngine;

namespace Elysium.Items.UI
{
    public class UsableFilter : IItemFilter
    {
        [SerializeField] private string name = "Usable";

        public string Name => name;

        public bool Evaluate(IItem _element)
        {
            return _element.IsUsable;
        }
    }
}
