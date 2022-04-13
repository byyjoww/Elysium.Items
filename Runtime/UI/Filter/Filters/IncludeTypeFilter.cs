namespace Elysium.Items.UI
{
    public abstract class IncludeTypeFilter<T> : IItemFilter where T : IItem
    {
        public abstract string Name { get; }

        public bool Evaluate(IItem _element)
        {
            return typeof(T).IsAssignableFrom(_element.GetType());
        }
    }
}
