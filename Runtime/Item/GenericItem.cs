using System;
using System.IO;
using UnityEngine;
using static System.Text.Encoding;

namespace Elysium.Items
{
    public class GenericItem : IItem
    {
        private Guid itemID = default;
        private Guid instanceID = default;
        private string itemName = default;
        private Sprite icon = default;
        private int maxStack = 1;
        private bool usable = false;        

        public Guid ItemID => itemID;
        public Guid InstanceID => instanceID;
        public string Name => itemName;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public bool IsUsable => usable;

        public GenericItem()
        {
            this.itemID = Guid.NewGuid();
            this.instanceID = Guid.NewGuid();
        }

        public GenericItem(string _itemID, string _instanceID)
        {
            this.itemID = Guid.Parse(_itemID);
            this.instanceID = Guid.Parse(_instanceID);
        }

        public override bool Equals(System.Object _item)
        {
            GenericItem item = _item as GenericItem;
            if (item == null) { return false; }
            return item.ItemID == ItemID && item.InstanceID == InstanceID;
        }

        public override int GetHashCode()
        {
            return instanceID.GetHashCode();
        }
    }
}