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

        void Use(IItemUser _user);
    }

    public interface IItem<T> : IItem where T : IItemUser
    {
        void Use(T _user);
    }
}
