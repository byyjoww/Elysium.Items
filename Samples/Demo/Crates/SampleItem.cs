using System;
using UnityEditor;
using UnityEngine;

namespace Elysium.Items.Samples.Crates
{
    public class SampleItem : IItem
    {
        public Guid ItemID { get; private set; } = Guid.NewGuid();
        public Guid InstanceID { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = "Sample Item";
        public string Description => "This is a sample item.";
        public Sprite Icon { get; private set; } = null;
        public int MaxStack { get; private set; } = 1;
        public bool IsUsable { get; private set; } = false;

        public SampleItem(string _name, string _icon)
        {
            Name = _name;
            Icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Packages/com.elysium.items/Samples/Demo/Crates/Textures/{_icon}.png");
        }

        public void Use(IItemStack _stack, IItemUser _user)
        {
            Debug.Log($"Item {Name} has been used");
        }
    }
}

