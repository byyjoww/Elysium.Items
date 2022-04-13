using Elysium.Core.Utils;
using Elysium.Items.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items.Samples
{
    public class SampleInventoryOpener : MonoBehaviour
    {
        [SerializeField] private SampleItemUser itemUser = default;
        [SerializeField] private GameObject inventoryPanel = default;
        [SerializeField] private ItemFilterConfigSO filterConfig = default;
        [SerializeField] private VisualInventorySlotPoolView view = default;
        [SerializeField] private VisualInventorySlotPoolView overrideView = default;
        [SerializeField] private VisualInventoryConfig inventoryConfig = default;
        [SerializeField] private TMPDropdownItemFilterer inventoryFilterer = default;

        private IInventory inventory = default;
        private IVisualInventory visualInventory = default;
        private IVisualInventory visualInventoryOverride = default;
        private IEnumerable<IItem> items = default;

        void Start()
        {
            items = new IItem[]
            {
                new SampleItem("item1", "t_item1", 1),
                new SampleItem("item2", "t_item2", 10),
                new SampleItem("item3", "t_item3", 1),
                new SampleConsumableItem("item4", "t_item4", 10),
                new SampleConsumableItem("item5", "t_item5", 1),
                new SampleConsumableItem("item6", "t_item6", 1),
            };

            inventory = LimitedInventory.New(new Capacity { Default = 21 }, new IItemStack[]
            {
                ItemStack.WithContents(items.ElementAt(0), 1),
                ItemStack.WithContents(items.ElementAt(1), 3),
                ItemStack.WithContents(items.ElementAt(2), 1),
                ItemStack.WithContents(items.ElementAt(3), 6),
                ItemStack.WithContents(items.ElementAt(4), 1),
                ItemStack.WithContents(items.ElementAt(5), 1),
            });

            visualInventory = new VisualInventory(inventoryConfig, inventoryFilterer, view, inventoryPanel);
            visualInventoryOverride = new VisualInventoryOverride(inventoryConfig, new StaticDefaultItemFilterer(), overrideView, inventoryPanel);

            OpenInventory();
        }

        public void GetRandomItem()
        {
            inventory.Add(items.Random(), 1);
        }

        public void OpenInventory()
        {
            visualInventoryOverride.Close();
            visualInventory.Open(inventory, filterConfig, new LoggedUseItemEvent(itemUser));
        }

        public void OpenInventoryOverride()
        {
            visualInventory.Close();
            visualInventoryOverride.Open(inventory, filterConfig, new LoggedUseItemEvent(itemUser));
        }
    }
}
