using Elysium.Core.Attributes;
using UnityEngine;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class VisualInventoryConfig
    {
        [SerializeField] private int numOfLastRowElements = 0;
        [SerializeField] private bool minNumOfSlotsByCapacity = true;
        [SerializeField, ConditionalField("minNumOfSlotsByCapacity", true)] private int minNumberOfSlots = 0;

        public int NumOfLastRowElements { get => numOfLastRowElements; }
        public bool MinNumOfSlotsByCapacity { get => minNumOfSlotsByCapacity; }
        public int MinNumberOfSlots { get => minNumberOfSlots; }
    }
}