using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Elysium.Utils;
using System;

namespace Elysium.Items
{
    public class UI_InventorySlot : MonoBehaviour
    {
        [Header("Icon")]
        [SerializeField] private Image iconImageComponent = default;
        [SerializeField] private Sprite emptySlotItemIcon = default;

        [Header("Count")]
        [SerializeField] private TMP_Text countTextComponent = default;
        [SerializeField] private GameObject countBackground = default;

        [Header("Button")]
        [SerializeField] private Button buttonActionComponent = default;
        [SerializeField] private TMP_Text buttonTextComponent = default;

        public ItemStack Stack { get; private set; }
        private InventorySO Inventory { get; set; }

        public void Setup(InventorySO _inventory, ItemStack _stack, ItemStackEventSO _useItemEvent)
        {
            Stack = _stack;
            Inventory = _inventory;

            if (_stack.IsNullOrEmpty())
            {
                SetupEmptySlot();
                return;
            }

            SetupActiveSlot(_stack, _useItemEvent);
        }

        public void AddActionToButton(UnityAction _action)
        {
            buttonActionComponent.onClick.AddListener(_action);
        }

        private void SetupEmptySlot()
        {
            SetCountPanelVisibility(false);
            SetButtonVisibility(false);

            SetItemCount(0);
            SetItemIcon(emptySlotItemIcon);

            var draggable = GetComponent<UI_Draggable>();
            var dropzone = GetComponent<UI_DropZone>();
            draggable.enabled = false;
            dropzone.OnReceiveDrop += OnItemDropped;
        }

        private void OnItemDropped(UI_Draggable _draggable)
        {
            var draggable = _draggable.GetComponent<UI_InventorySlot>();
            if (draggable == null) { throw new System.Exception("draggable was dropped without containing a UI_InventorySlot script"); }

            Debug.Log("swapping stack contents");
            Inventory.SwapStackContents(draggable.Stack, Stack);
        }

        private void SetupActiveSlot(ItemStack _stack, ItemStackEventSO _useItemEvent)
        {
            SetCountPanelVisibility(true);

            SetItemCount(_stack.Amount);
            SetItemIcon(_stack.InventoryElement.Icon);
            
            SetButtonText("USE");
            SetButtonVisibility(_stack.InventoryElement.IsUsable);

            UnityAction action = () => _useItemEvent.Raise(_stack);
            SetButtonAction(action);

            var draggable = GetComponent<UI_Draggable>();
            var dropzone = GetComponent<UI_DropZone>();
            draggable.enabled = true;
            dropzone.OnReceiveDrop += OnItemDropped;
        }

        private void SetItemCount(int _count)
        {
            countTextComponent.text = $"{_count}";
        }

        private void SetItemIcon(Sprite _icon)
        {
            iconImageComponent.sprite = _icon;
        }

        private void SetButtonText(string _text)
        {
            if (buttonTextComponent == null) { return; }
            buttonTextComponent.text = _text;
        }

        private void SetButtonAction(UnityAction _action)
        {
            buttonActionComponent.onClick.RemoveAllListeners();
            buttonActionComponent.onClick.AddListener(_action);
        }        

        private void SetCountPanelVisibility(bool _state)
        {
            countBackground.SetActive(_state);
        }

        private void SetButtonVisibility(bool _state)
        {
            buttonActionComponent.enabled = _state;
            // buttonActionComponent.gameObject.SetActive(_state);
        }
    }
}