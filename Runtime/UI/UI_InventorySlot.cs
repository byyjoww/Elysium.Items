using Elysium.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.Items.UI
{
    public class UI_InventorySlot : MonoBehaviour
    {
        public class Config
        {
            public IInventory Inventory { get; set; }
            public IItemStack Stack { get; set; }
            public IUseItemEvent Event { get; set; }
        }

        [Header("Icon")]
        [SerializeField] protected Image iconImageComponent = default;

        [Header("Count")]
        [SerializeField] protected TMP_Text countTextComponent = default;
        [SerializeField] protected GameObject countBackground = default;

        [Header("Button")]
        [SerializeField] protected Button buttonActionComponent = default;
        [SerializeField] protected TMP_Text buttonTextComponent = default;
                
        protected IItemStack stack { get; private set; }
        protected IInventory inventory { get; private set; }
        protected IUseItemEvent useItemEvent { get; private set; }

        public bool Draggable { get => draggable.enabled; set => draggable.enabled = value; }
        protected UI_Draggable draggable { get; set; }
        protected UI_DropZone dropzone { get; set; }

        protected virtual void Awake()
        {
            draggable = GetComponent<UI_Draggable>();
            dropzone = GetComponent<UI_DropZone>();
            dropzone.OnReceiveDrop += OnItemDropped;
            draggable.enabled = false;
        }

        public virtual void Setup(Config _config)
        {
            this.stack = _config.Stack;
            this.inventory = _config.Inventory;
            this.useItemEvent = _config.Event;

            SetupQuantity(stack.Quantity);
            SetupIcon(stack.Item.Icon);
            SetDraggable(false);            
            SetupButton();
        }

        protected virtual void SetupQuantity(int _count)
        {
            countTextComponent.text = $"{_count}";
            countBackground.SetActive(_count > 1);
        }

        protected virtual void SetupIcon(Sprite _icon)
        {
            iconImageComponent.sprite = _icon;
        }

        public virtual void SetDraggable(bool _canDrag)
        {
            draggable.enabled = _canDrag;
        }

        protected virtual void SetupButton()
        {
            if (buttonTextComponent != null) { buttonTextComponent.text = "USE"; }

            buttonActionComponent.enabled = useItemEvent != null;
            if (buttonActionComponent.enabled)
            {
                buttonActionComponent.onClick.AddListener(delegate { useItemEvent.Raise(inventory, stack); });
            }
        }

        protected virtual void OnItemDropped(UI_Draggable _draggable)
        {
            var slot = _draggable.GetComponent<UI_InventorySlot>();
            if (slot == null) { throw new System.Exception("draggable was dropped without containing a UI_InventorySlot script"); }

            Debug.Log("swapping stack contents");
            inventory.Swap(slot.stack, stack);
        }
    }
}