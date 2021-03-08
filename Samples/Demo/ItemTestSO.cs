using Elysium.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTestSO", menuName = "Scriptable Objects/Item/ItemSO")]
public class ItemTestSO : ScriptableObject, IInventoryElement
{
    public bool isStackable = false;

    public string ItemName => "demo";
    public bool IsStackable => isStackable;
}
