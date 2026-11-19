public class Potion : ItemStack
{
    public Potion(Item item, int count) : base(item, count)
    {
        _item = item;
    }
}