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
        bool IsStackable { get; }
        // string Description { get; }
        // Sprite Icon { get; }
        // int Value { get; }
        // Action Action { get; }
        // void Activate();
    }

    [System.Serializable]
    public class IInventoryElementDatabase : IDatabase<IInventoryElement> { }
}