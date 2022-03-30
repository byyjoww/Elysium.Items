using System;
using System.IO;
using UnityEngine;
using static System.Text.Encoding;

namespace Elysium.Items
{
    public class Item : IItem
    {
        private string itemName = default;
        private Sprite icon = default;
        private int maxStack = 1;
        private bool usable = false;        

        public Guid ItemID => Guid.NewGuid();
        public Guid InstanceID => Guid.NewGuid();
        public string Name => itemName;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public bool IsUsable => usable;

        public Item()
        {

        }
    }
}