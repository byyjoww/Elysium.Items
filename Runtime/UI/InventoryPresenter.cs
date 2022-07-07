using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items.UI
{
    public class InventoryPresenter : IInventoryPresenter
    {        
        protected InventoryPresenterConfig config = default;
        protected IItemFilterer filter = default;
        protected IInventoryViewSlotSpawner view = default;
        protected GameObject inventoryPanel = default;
        protected IInventory inventory = new NullInventory();
        protected IUseItemEvent useItemEvent = new NullUseItemEvent();
        protected bool open = false;

        protected virtual int NumOfSlots => config.MinNumOfSlotsByCapacity ? inventory.Items.Stacks.Count() : Mathf.Max(config.MinNumberOfSlots, inventory.Items.Stacks.Count());

        public InventoryPresenter(InventoryPresenterConfig _config, IItemFilterer _filter, IInventoryViewSlotSpawner _view, GameObject _inventoryPanel)
        {
            this.config = _config;
            this.filter = _filter;
            this.view = _view;
            this.inventoryPanel = _inventoryPanel;
        }

        public void Show(IInventory _inventory, IItemFilterConfig _config, IUseItemEvent _event)
        {
            if (open) { return; }
            this.useItemEvent = _event;
            this.inventory = _inventory;
            inventoryPanel.SetActive(true);
            Register(_config);
            Spawn();
            open = true;
        }

        public virtual void Hide()
        {
            if (!open) { return; }
            Deregister();
            view.Set(0);
            inventoryPanel.SetActive(false);
            open = false;
        }

        protected virtual void Register(IItemFilterConfig _config)
        {
            filter.Init(_config);
            filter.OnValueChanged += Spawn;
            inventory.OnValueChanged += Spawn;
        }

        protected virtual void Deregister()
        {
            filter.End();
            filter.OnValueChanged -= Spawn;
            inventory.OnValueChanged -= Spawn;
        }

        protected void Spawn()
        {
            int numOfSlots = GetNumberOfInventorySlots(NumOfSlots, config.NumOfLastRowElements);
            var objs = view.Set(numOfSlots);
            var ordered = inventory.Items.Stacks.OrderByDescending(x => !Invisible(x)).ToList();
            for (int i = 0; i < numOfSlots; i++)
            {
                IInventoryViewSlot slot = objs.ElementAt(i);
                IItemStack stack = GetStack(ordered, i);
                ConfigureSlot(slot, stack);
            }
        }

        protected virtual void ConfigureSlot(IInventoryViewSlot _slot, IItemStack _stack)
        {
            _slot.Setup(new IInventoryViewSlotConfig
            {
                Stack = _stack,
                Event = _stack.Item.IsUsable ? useItemEvent : new NullUseItemEvent(),
                CanSwap = config.EnableSwapping,
            });
        }

        protected virtual IItemStack GetStack(List<IItemStack> _stacks, int _index)
        {
            IItemStack itemFromStack = _stacks.Count <= _index ? new NullItemStack() : _stacks[_index];
            if (Invisible(itemFromStack)) { return new NullItemStack(); }
            return itemFromStack;
        }

        protected virtual int GetNumberOfInventorySlots(int _current, int _min)
        {
            if (_min <= 0) { return _current; }
            return _current + ((_min - _current % _min) % _min);
        }

        protected virtual bool Invisible(IItemStack _stack)
        {
            return (config.HideEmptySlots && _stack.IsEmpty) || !filter.Evaluate(_stack.Item);
        }
    }
}