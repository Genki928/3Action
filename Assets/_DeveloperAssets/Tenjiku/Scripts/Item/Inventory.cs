using System.Collections.Generic;

public class Inventory
{
    public List<ItemStack> Stack => _items;
    List<ItemStack> _items = new();

    public void AddItem(ItemStack item)
    {
        // ŒÂ”‚ð‘«‚·
        var findItemIndex = _items.FindIndex(x => x.Item.Name == item.Item.Name);
        if (findItemIndex != -1)
        {
            _items[findItemIndex].AddItem(item.Count);
        }
        else
        {
            _items.Add(item);
        }
    }

    public void RemoveItem(ItemStack item)
    {
        // ŒÂ”‚ðˆø‚­
        var findItemIndex = _items.FindIndex(x => x.Item.Name == item.Item.Name);
        if (findItemIndex != -1)
        {
            _items[findItemIndex].RemoveItem(item.Count);
        }
    }
}