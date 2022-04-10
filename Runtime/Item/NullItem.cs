using System;
using System.IO;
using UnityEngine;

namespace Elysium.Items
{
    public class NullItem : IItem
    {
        public Sprite icon = default;

        public Guid ItemID => Guid.Empty;
        public Guid InstanceID => Guid.Empty;
        public string Name => "null";
        public string Description => "";
        public Sprite Icon => icon;
        public int MaxStack => 0;
        public bool IsUsable => false;

        public NullItem()
        {
            icon = Resources.Load<Sprite>("empty");
        }

        public void Use(IItemStack _stack, IItemUser _user)
        {
            
        }
    }
}
