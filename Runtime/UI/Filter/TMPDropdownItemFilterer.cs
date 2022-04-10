using Elysium.Core.Attributes;
using Elysium.Core.Utils;
using Elysium.Core.Utils.Filters;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    [System.Serializable]
    public class TMPDropdownItemFilterer : IItemFilterer
    {
        [SerializeField] private bool enabled = false;
        [SerializeField, ConditionalField("enabled")] private TMP_Dropdown dropdown = default;

        protected IItemFilterConfig config = default;
        protected IFilter<IItem> active = default;
        public IFilter<IItem> Active => active;

        public event UnityAction OnSelectionChanged = delegate { };
        public event UnityAction OnValueChanged = delegate { };

        public void Init(IItemFilterConfig _config)
        {
            if (!enabled)
            {
                active = new NullFilter<IItem>();
                return; 
            }

            this.config = _config;
            dropdown.onValueChanged.AddListener(ChangeFilter);
            config.OnValueChanged += Update;
            Update();
            SetDefaultFilter();

            dropdown.gameObject.SetActive(_config.Visible);
        }

        public bool Evaluate(IItem _item)
        {
            return Active.Evaluate(_item);
        }

        public void End()
        {
            if (!enabled) { return; }
            dropdown.onValueChanged.RemoveAllListeners();
            config.OnValueChanged -= Update;
            this.config = null;
        }

        public void Update()
        {
            dropdown.ClearOptions();

            List<string> filterNames = config.Filters.Select(x => x.Name).ToList();
            dropdown.AddOptions(filterNames);
            OnSelectionChanged?.Invoke();
        }

        public void SetDefaultFilter()
        {
            active = config.Default;
            OnValueChanged?.Invoke();
        }

        private void ChangeFilter(int _index)
        {
            active = config.Filters[_index];
            OnValueChanged?.Invoke();
        }
    }
}
