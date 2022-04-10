using UnityEngine;

namespace Elysium.Items.Samples
{
    public class SampleConsumableItem : SampleItem
    {
        public SampleConsumableItem(string _name, string _icon, int _maxStacks) : base(_name, _icon, _maxStacks)
        {

        }

        public override void Use(IItemStack _stack, IItemUser _user)
        {
            Debug.Log("Using consumable item");
            _stack.Remove(1);
            base.Use(_stack, _user);
        }
    }
}

