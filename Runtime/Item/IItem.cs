using System;
using System.IO;
using UnityEngine;

namespace Elysium.Items
{
    public interface IItem
    {
        Guid ItemID { get; }
        Guid InstanceID { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        int MaxStack { get; }
        bool IsUsable { get; }

        void Use(IItemStack _stack, IItemUser _user);
    }

    public interface IItem<T> : IItem where T : IItemUser
    {
        void Use(IItemStack _stack, T _user);
    }
}
