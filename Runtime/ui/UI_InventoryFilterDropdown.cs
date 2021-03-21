using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Elysium.Items
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UI_InventoryFilterDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown = default;
        [SerializeField] private UI_Inventory inventory = default;

        private void Awake()
        {
            if (dropdown == null) { dropdown = GetComponent<TMP_Dropdown>(); }
            dropdown.AddOptions(InventoryFilters.GetAllFilterNames());
            dropdown.onValueChanged.AddListener(inventory.ChangeActiveFilter);
        }

        private void OnValidate()
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }
    }
}