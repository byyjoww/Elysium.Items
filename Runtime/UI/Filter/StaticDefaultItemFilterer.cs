using Elysium.Core.Utils.Filters;
using UnityEngine.Events;

namespace Elysium.Items.UI
{
    public class StaticDefaultItemFilterer : IItemFilterer
    {
        protected IItemFilterConfig config = default;
        protected IFilter<IItem> filter = default;

        public event UnityAction OnValueChanged = delegate { };

        public StaticDefaultItemFilterer()
        {

        }

        public void Init(IItemFilterConfig _config)
        {
            this.config = _config;
            config.OnValueChanged += Update;
            Update();
        }

        public bool Evaluate(IItem _item)
        {
            return filter.Evaluate(_item);
        }

        public void End()
        {
            config.OnValueChanged -= Update;
        }

        public void Update()
        {
            this.filter = config.Default;
        }
    }
}