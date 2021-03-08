using UnityEngine;
using Elysium.Utils.Attributes;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemStack
    {
        [RequireInterface(typeof(IInventoryElement))]
        public ScriptableObject Element;
        public int Amount;

        public IInventoryElement InventoryElement => Element as IInventoryElement;

        public ItemStack(IInventoryElement _element, int _amount)
        {
            Element = _element as ScriptableObject;
            Amount = _amount;
        }
    }
}