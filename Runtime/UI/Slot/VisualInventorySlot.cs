using Elysium.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Elysium.Items.UI
{
    public class VisualInventorySlot : MonoBehaviour, IVisualInventorySlot, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Icon")]
        [SerializeField] protected Image icon = default;

        [Header("Count")]
        [SerializeField] protected TMP_Text stackAmountText = default;
        [SerializeField] protected GameObject stackAmountBackground = default;

        [Header("Button")]
        [SerializeField] protected Button useItemButton = default;

        protected IItemStack stack = default;
        protected IUseItemEvent useItemEvent = default;
        protected IDraggable draggable = default;
        protected IDroppable droppable = default;
        protected IPointerResolver pointerResolver = new PointerResolver();
        protected IHoverResolver hoverResolver = new HoverResolver();

        public event UnityAction OnDragBegin = delegate { };
        public event UnityAction OnDragEnd = delegate { };
        public event UnityAction<IDraggable> OnDraggableEnter = delegate { };
        public event UnityAction<IDraggable> OnDraggableExit = delegate { };
        public event UnityAction<IDraggable> OnReceiveDrop = delegate { };

        private void Awake()
        {
            draggable = gameObject.GetComponent<DuplicatedDraggable>();
            droppable = gameObject.GetComponent<Droppable>();

            if (draggable is null) { draggable = gameObject.AddComponent<DuplicatedDraggable>(); }
            if (droppable is null) { droppable = gameObject.AddComponent<Droppable>(); }

            draggable.CanDrag = false;
            droppable.CanDrop = false;

            droppable.OnReceiveDrop += RequestStackSwap;

            pointerResolver.OnClick += OnClick;
            pointerResolver.OnHoldStart += OnHoldStart;
            pointerResolver.OnHoldEnd += OnHoldEnd;
            draggable.OnDragBegin += pointerResolver.OnHoldOverride;

            hoverResolver.OnHoverStart += OnHoverStart;
            hoverResolver.OnHoverEnd += OnHoverEnd;
            Draggable.OnAnyDragBegin += hoverResolver.OnPointerExit;

            enabled = false;
        }

        public virtual void Setup(VisualInventorySlotConfig _config)
        {
            enabled = true;
            this.stack = _config.Stack;
            this.useItemEvent = _config.Event;

            gameObject.name = stack.Item.Name;
            draggable.CanDrag = !_config.Stack.IsEmpty;
            droppable.CanDrop = true;
            SetupQuantity(stack.Quantity);
            SetupIcon(stack.Item.Icon);
        }

        public void Swap(IItemStack _stack)
        {
            _stack.SwapContents(this.stack);
        }

        #region EVENTS
        protected virtual void OnHoverStart()
        {
            // Debug.Log($"{gameObject.name} has started been hovered over");
        }

        protected virtual void OnHoverEnd()
        {
            // Debug.Log($"{gameObject.name} has stopped been hovered over");
        }

        protected virtual void OnHoldStart()
        {
            // Debug.Log($"{gameObject.name} has started been held");
        }

        protected virtual void OnHoldEnd()
        {
            // Debug.Log($"{gameObject.name} has stopped been held");
        }

        protected virtual void OnClick()
        {
            // Debug.Log($"{gameObject.name} was clicked");
            useItemEvent.Raise(stack, 1);
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

        private void RequestStackSwap(IDraggable _draggable)
        {
            if (_draggable.gameObject.TryGetComponent(out IVisualInventorySlot slot))
            {
                slot.Swap(stack);
            }
        }

        public void OnPointerEnter(PointerEventData _data)
        {
            if (Draggable.DraggablesInDrag.Count > 0) { return; }
            hoverResolver.OnPointerEnter();
        }

        public void OnPointerExit(PointerEventData _data)
        {
            hoverResolver.OnPointerExit();
        }

        public void OnPointerDown(PointerEventData _data)
        {
            pointerResolver.OnPointerDown();
        }

        public void OnPointerUp(PointerEventData _data)
        {
            pointerResolver.OnPointerUp();
        }

        private void OnDestroy()
        {            
            droppable.OnReceiveDrop -= RequestStackSwap;

            pointerResolver.OnClick -= OnClick;
            pointerResolver.OnHoldStart -= OnHoldStart;
            pointerResolver.OnHoldEnd -= OnHoldEnd;
            draggable.OnDragBegin -= pointerResolver.OnHoldOverride;

            hoverResolver.OnHoverStart -= OnHoverStart;
            hoverResolver.OnHoverEnd -= OnHoverEnd;
            Draggable.OnAnyDragBegin -= hoverResolver.OnPointerExit;
        }        
    }    
}