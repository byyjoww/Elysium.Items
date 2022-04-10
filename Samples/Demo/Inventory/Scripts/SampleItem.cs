﻿using System;
using UnityEditor;
using UnityEngine;

namespace Elysium.Items.Samples
{
    public class SampleItem : IItem
    {
        public Guid ItemID { get; private set; } = Guid.NewGuid();
        public Guid InstanceID { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = "Sample Item";
        public string Description { get; private set; } = "";
        public Sprite Icon { get; private set; } = null;
        public int MaxStack { get; private set; } = 1;
        public bool IsUsable { get; private set; } = true;

        public SampleItem(string _name, string _icon, int _maxStacks)
        {
            Name = _name;
            MaxStack = _maxStacks;
            Icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Packages/com.elysium.items/Samples/Demo/Inventory/Textures/{_icon}.png");
        }

        public virtual void Use(IItemStack _stack, IItemUser _user)
        {
            Debug.Log($"Item {Name} has been used by {_user}");
            (_user as SampleItemUser).AddGold(10);
        }
    }
}

