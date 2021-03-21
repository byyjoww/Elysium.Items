using Elysium.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public interface IInventoryElement
    {
        string ItemName { get; }
        Sprite Icon { get; }
        bool IsStackable { get; }
        bool IsUsable { get; }
        void Activate(ItemStack _stack, IInventoryUser _user);
    }

    [System.Serializable]
    public class IInventoryElementDatabase : IDatabase<IInventoryElement> { }
}