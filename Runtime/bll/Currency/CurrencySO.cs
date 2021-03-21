using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Elysium.Utils.Attributes;
using Elysium.Core;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "CurrencySO", menuName = "Scriptable Objects/Items/Currency")]
    public class CurrencySO : SavableLongSO
    {
        [Separator("Currency Details", true)]
        public string CurrencyName;

        public bool HaveEnoughCurrency(long amount)
        {
            return amount <= Value;
        }

        public void GetCurrency(long amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
            }

            Value += amount;
            Debug.Log($"Player gained {amount} {CurrencyName}.");
        }

        public bool SpendCurrency(long amount)
        {
            if (!HaveEnoughCurrency(amount))
            {
                Debug.Log($"Player has insufficient {CurrencyName}.");
                return false;
            }

            if (amount < 0)
            {
                throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
            }

            Value -= amount;
            Debug.Log($"Player spent {amount} {CurrencyName}.");

            return true;
        }
    }
}