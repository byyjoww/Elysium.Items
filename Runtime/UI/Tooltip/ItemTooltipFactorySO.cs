using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    [CreateAssetMenu(fileName = "ItemTooltipFactorySO_", menuName = "Scriptable Objects/Items/Tooltip Factory")]
    public class ItemTooltipFactorySO : FactorySO<ItemTooltip>, ITooltip
    {
        private ItemTooltip tooltip = default;

        public void Open(IItem _item, UnityAction _action)
        {
            tooltip = Create();
            tooltip.Open(_item, _action);
            tooltip.OnClose += DisposeTooltip;
        }

        public void Close()
        {            
            tooltip.Close();
            DisposeTooltip();
        }

        private void DisposeTooltip()
        {
            tooltip.OnClose -= Close;
            Destroy(tooltip.gameObject);
        }

        private void OnDisable()
        {
            if (tooltip == null) { return; }
            Close();
        }
    }    
}
