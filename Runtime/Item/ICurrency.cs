using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Items
{
    public interface ICurrency
    {
        string Name { get; }
        long Amount { get; }

        event UnityAction OnValueChanged;

        bool Has(long amount);
        void Gain(long amount);
        void Deduct(long _amount);
        bool Spend(long amount);
    }
}
