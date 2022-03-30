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
        Sprite Icon { get; }
        int MaxStack { get; }
        bool IsUsable { get; }
    }
}
