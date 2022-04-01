using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class Currency : ICurrency
    {
        private string name = default;
        private long amount = 0;

        public string Name => name;
        public virtual long Amount
        {
            get => this.amount;
            set
            {
                long prev = this.amount;
                this.amount = value;
                if (prev != this.amount) { OnValueChanged?.Invoke(); }                
            }
        }

        public event UnityAction OnValueChanged = delegate { };

        public Currency(string _name, long _amount)
        {
            this.name = _name;
            this.amount = _amount;
        }

        public bool Has(long _amount)
        {
            return _amount <= this.Amount;
        }

        public void Gain(long _amount)
        {
            ValidatePositiveAmount(_amount);
            Amount += _amount;
            Debug.Log($"Player gained {_amount} {Name}.");
        }

        public void Deduct(long _amount)
        {
            ValidatePositiveAmount(_amount);
            Amount -= _amount;
            Debug.Log($"Player lost {_amount} {Name}.");
        }

        public bool Spend(long _amount)
        {
            ValidatePositiveAmount(_amount);
            if (!Has(_amount))
            {
                Debug.Log($"Player has insufficient {Name}.");
                return false;
            }

            Deduct(_amount);
            return true;
        }

        private void ValidatePositiveAmount(long _amount)
        {
            if (_amount < 0)
            {
                throw new System.ArgumentException($"Invalid value for <amount> ({_amount}). Value must be a positive integer.");
            }
        }
    }
}
