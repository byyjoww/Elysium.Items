using Elysium.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Elysium.Items.UI
{
    public class VisualInventorySlot : MonoBehaviour, IVisualInventorySlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Icon")]
        [SerializeField] protected Image icon = default;

        [Header("Count")]
        [SerializeField] protected TMP_Text stackAmountText = default;
        [SerializeField] protected GameObject stackAmountBackground = default;

        [Header("Button")]
        [SerializeField] protected Button useItemButton = default;

        protected IInventory inventory { get; private set; }
        protected IItemStack stack { get; private set; }
        protected IUseItemEvent useItemEvent { get; private set; }
        protected IDraggable draggable { get; private set; }
        protected IDroppable droppable { get; private set; }

        public event UnityAction OnDragBegin = delegate { };
        public event UnityAction OnDragEnd = delegate { };
        public event UnityAction<IDraggable> OnDraggableEnter = delegate { };
        public event UnityAction<IDraggable> OnDraggableExit = delegate { };
        public event UnityAction<IDraggable> OnReceiveDrop = delegate { };

        private void Awake()
        {
            draggable = gameObject.AddComponent<DuplicatedDraggable>();
            droppable = gameObject.AddComponent<Droppable>();

            draggable.CanDrag = false;
            droppable.CanDrop = false;
            droppable.OnReceiveDrop += RequestStackSwap;

            enabled = false;
        }

        public virtual void Setup(VisualInventorySlotConfig _config)
        {
            enabled = true;
            this.inventory = _config.Inventory;
            this.stack = _config.Stack;

            gameObject.name = stack.Item.Name;
            draggable.CanDrag = !_config.Stack.IsEmpty;
            SetupQuantity(stack.Quantity);
            SetupIcon(stack.Item.Icon);
            SetupButton();
        }

        public void Swap(IItemStack _stack)
        {
            Debug.Log($"swapping {stack.Item.Name} with {_stack.Item.Name}");
            inventory.Swap(stack, _stack);
        }

        #region EVENTS
        protected virtual void OnHoverStart()
        {
            Debug.Log($"{gameObject.name} has started been hovered over");
        }

        protected virtual void OnHoverEnd()
        {
            Debug.Log($"{gameObject.name} has stopped been hovered over");
        }

        protected virtual void OnHoldStart()
        {
            Debug.Log($"{gameObject.name} has started been held");
        }

        protected virtual void OnHoldEnd()
        {
            Debug.Log($"{gameObject.name} has stopped been held");
        }

        protected virtual void OnClick()
        {
            Debug.Log($"{gameObject.name} was clicked");
            useItemEvent.Raise(inventory, stack);
        }
        #endregion

        protected virtual void SetupIcon(Sprite _icon)
        {
            icon.sprite = _icon;
        }

        protected virtual void SetupQuantity(int _count)
        {
            stackAmountText.text = $"{_count}";
            stackAmountBackground.SetActive(_count > 1);
        }

        protected virtual void SetupButton()
        {
            useItemButton.onClick.RemoveAllListeners();
            useItemButton.onClick.AddListener(OnClick);
        }

        private void RequestStackSwap(IDraggable _draggable)
        {
            if (_draggable.gameObject.TryGetComponent(out IVisualInventorySlot slot))
            {
                slot.Swap(stack);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverStart();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverEnd();
        }

        private void OnDestroy()
        {
            droppable.OnReceiveDrop -= RequestStackSwap;
        }        
    }
}