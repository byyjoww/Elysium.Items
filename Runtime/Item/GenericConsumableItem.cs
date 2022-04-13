namespace Elysium.Items
{
    public class GenericConsumableItem : GenericItem
    {
        public override void Use(IItemStack _stack, IItemUser _user)
        {
            _stack.Remove(1);
            base.Use(_stack, _user);
        }
    }
}