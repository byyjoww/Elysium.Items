using System;
using System.Timers;
using UnityEngine;

namespace Elysium.Items.UI
{
    public class CooldownUseItemEvent : DefaultUseItemEvent
    {
        private Timer timer = default;

        public CooldownUseItemEvent(IItemUser _user, TimeSpan _cooldown) : base(_user)
        {
            timer = new Timer(_cooldown.TotalMilliseconds);
            timer.AutoReset = false;
        }

        public override void Raise(IItemStack _stack, int _quantity)
        {
            if (timer.Enabled)
            {
                Debug.Log($"Use item event timer is still on cooldown");
                return; 
            }

            timer.Start();
            Debug.Log($"Stack containing item {_stack.Item.Name} was used");
            base.Raise(_stack, _quantity);
        }
    }
}
