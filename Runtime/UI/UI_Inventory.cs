using Elysium.Core.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items.UI
{
    public class UI_Inventory : MonoBehaviour
    {
        [Separator("Default", true)]
        [SerializeField] protected InventorySO defaultInventory = default;
        [SerializeField] protected UseItemEventSO defaultUseItemEvent = default;
        [SerializeField] protected InventoryFilterConfigSO defaultFilterConfig = default;

        [Separator("Settings", true)]
        [SerializeField] protected int numOfLastRowElements = 0;
        [SerializeField] protected bool minNumOfSlotsByCapacity = true;
        [SerializeField, ConditionalField("minNumOfSlotsByCapacity", true)] protected int minNumberOfSlots = 0;

        [Separator("View")]
        [SerializeField] protected InventorySlotPoolView view = default;

        [Separator("Filters", true)]
        [SerializeField] protected InventoryFilter filter = default;

        [Separator("Tooltip", true)]
        [SerializeField] protected InventoryTooltip tooltip = default;

        protected virtual int NumOfSlots => minNumOfSlotsByCapacity ? activeInventory.Items.Stacks.Count() : Mathf.Max(minNumberOfSlots, activeInventory.Items.Stacks.Count());
        protected IInventory activeInventory { get; set; }
        protected IUseItemEvent activeEvent { get; set; }
        protected bool triggerEventOnAllItems { get; set; }

        public void Open() => OpenInternal(defaultInventory, defaultFilterConfig, defaultUseItemEvent, false);
        public void Open(InventorySO _inventory) => OpenInternal(_inventory, defaultFilterConfig, defaultUseItemEvent, true);
        public void Open(InventorySO _inventory, IItemFilterConfig _config) => OpenInternal(_inventory, _config, defaultUseItemEvent, true);
        public void Open(InventorySO _inventory, IUseItemEvent _event) => OpenInternal(_inventory, defaultFilterConfig, _event, true);
        public void Open(IItemFilterConfig _config) => OpenInternal(defaultInventory, _config, defaultUseItemEvent, true);
        public void Open(IItemFilterConfig _config, IUseItemEvent _event) => OpenInternal(defaultInventory, _config, _event, true);
        public void Open(IUseItemEvent _event) => OpenInternal(defaultInventory, defaultFilterConfig, _event, true);
        public void Open(InventorySO _inventory, IItemFilterConfig _config, IUseItemEvent _event) => OpenInternal(_inventory, _config, _event, true);

        protected virtual void OpenInternal(InventorySO _inventory, IItemFilterConfig _config, IUseItemEvent _event, bool _triggerEventOnAllItems)
        {
            this.triggerEventOnAllItems = _triggerEventOnAllItems;
            this.activeEvent = _event;
            this.activeInventory = _inventory;
            gameObject.SetActive(true);
            Register(_config);
            Spawn();
        }

        public virtual void Close()
        {
            Deregister();
            gameObject.SetActive(false);
        }

        protected virtual void Register(IItemFilterConfig _config)
        {
            filter.Init(_config);
            filter.OnValueChanged += Spawn;
            activeInventory.OnValueChanged += Spawn;
            tooltip.OnValueChanged += Spawn;
        }

        protected virtual void Deregister()
        {
            filter.End();
            filter.OnValueChanged -= Spawn;
            activeInventory.OnValueChanged -= Spawn;
            tooltip.OnValueChanged -= Spawn;
        }

        protected void Spawn()
        {
            int numOfSlots = GetNumberOfInventorySlots(NumOfSlots, numOfLastRowElements);
            var objs = view.Set(numOfSlots);
            var ordered = activeInventory.Items.Stacks.OrderByDescending(x => !Invisible(x)).ToList();
            for (int i = 0; i < numOfSlots; i++)
            {
                UI_InventorySlot slot = objs[i];
                IItemStack stack = GetStack(ordered, i);
                slot.Setup(new UI_InventorySlot.Config
                {
                    Inventory = activeInventory,
                    Stack = stack,
                    Event = new UseItemEventFactory(stack, activeEvent, triggerEventOnAllItems, tooltip.Tooltip, tooltip.Show).Create(),
                });
            }
        }

        protected virtual IItemStack GetStack(List<IItemStack> _stacks, int _index)
        {
            IItemStack itemFromStack = _stacks.Count <= _index ? new NullItemStack() : _stacks[_index];
            if (Invisible(itemFromStack)) { return itemFromStack; }
            return itemFromStack;
        }

        protected virtual bool Invisible(IItemStack _stack)
        {
            return _stack.IsEmpty || !filter.Active.Evaluate(_stack.Item);
        }

        protected virtual int GetNumberOfInventorySlots(int _current, int _min)
        {
            if (_min <= 0) { return _current; }
            return _current + ((_min - _current % _min) % _min);
        }

        protected virtual void OnValidate()
        {
            if (numOfLastRowElements < 0) { numOfLastRowElements = 0; }
        }
    }
}