using UnityEngine;
using Elysium.Utils.Attributes;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemStack
    {
        [RequireInterface(typeof(IInventoryElement))]
        public ScriptableObject Element = null;
        public int Amount = 0;
        
        public IInventoryElement InventoryElement => Element as IInventoryElement;

        public static ItemStack Empty()
        {
            return new ItemStack();
        }

        public static ItemStack WithContents(IInventoryElement _item, int _amount)
        {
            var stack = new ItemStack();
            stack.SetContents(_item, _amount);
            return stack;
        }

        public void SetContents(IInventoryElement _item, int _amount)
        {
            Element = (ScriptableObject)_item;
            Amount = _amount;
        }
    }

    public static class ItemStackExtensions
    {
        public static bool IsNullOrEmpty(this ItemStack _stack)
        {
            return _stack == null || _stack.Element == null;
        }
    }
}