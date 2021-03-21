using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public abstract class ItemSO : ScriptableObject, IInventoryElement
    {
        [SerializeField] protected string itemName = default;
        [SerializeField] protected Sprite icon = default;
        [SerializeField] protected bool isStackable = default;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public bool IsStackable => isStackable;
        public abstract bool IsUsable { get; }        

        public virtual void Activate(ItemStack _stack, IInventoryUser _user)
        {

        }
    }
}