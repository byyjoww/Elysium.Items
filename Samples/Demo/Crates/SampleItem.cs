using System;
using UnityEditor;
using UnityEngine;

namespace Elysium.Items.Samples.Crates
{
    public class SampleItem : IItem
    {
        private string name = default;
        private Sprite icon = default;

        public Guid ItemID { get; private set; } = Guid.NewGuid();
        public Guid InstanceID { get; private set; } = Guid.NewGuid();
        public string Name => name;
        public Sprite Icon => icon;
        public int MaxStack { get; private set; } = 1;
        public bool IsUsable { get; private set; } = false;

        public SampleItem(string _name, string _icon)
        {
            name = _name;
            icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Packages/com.elysium.items/Samples/Demo/Crates/Textures/{_icon}.png");
        }
    }
}

