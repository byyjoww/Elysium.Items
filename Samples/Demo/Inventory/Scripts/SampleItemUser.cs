using System;
using TMPro;
using UnityEngine;

namespace Elysium.Items.Samples
{
    public class SampleItemUser : MonoBehaviour, IItemUser
    {
        [SerializeField] private ICurrency gold = new Currency("Gold", 0);
        [SerializeField] private TMP_Text goldAmount = default;

        public void AddGold(long _amount)
        {
            gold.Gain(_amount);
        }

        private void OnEnable()
        {
            gold.OnValueChanged += UpdateGoldAmount;
            UpdateGoldAmount();
        }

        private void OnDisable()
        {
            gold.OnValueChanged -= UpdateGoldAmount;
        }

        private void UpdateGoldAmount()
        {
            goldAmount.text = $"Gold: {gold.Amount}";
        }        
    }
}

