using Elysium.Core.Attributes;
using UnityEngine;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class VisualInventoryConfig
    {
        [SerializeField] private bool enableSwapping = true;
        [SerializeField] private bool hideEmptySlots = false;
        [SerializeField] private int numOfLastRowElements = 0;
        [SerializeField] private bool minNumOfSlotsByCapacity = true;
        [ConditionalField("minNumOfSlotsByCapacity", true)]
        [SerializeField] private int minNumberOfSlots = 0;

        public bool EnableSwapping => enableSwapping;
        public bool HideEmptySlots => hideEmptySlots;
        public int NumOfLastRowElements => numOfLastRowElements;
        public bool MinNumOfSlotsByCapacity => minNumOfSlotsByCapacity;
        public int MinNumberOfSlots => minNumberOfSlots;
    }
}