using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<ItemStack> Stack => _items;
    List<ItemStack> _items = new();

    public event Action<Inventory> OnGetItem;

    public void AddItem(ItemStack item, int count = 1)
    {
        // 個数を足す
        var findItemIndex = _items.FindIndex(x => x.Item.Name == item.Item.Name);
        if (findItemIndex != -1)
        {
            // アイテムを増加
            _items[findItemIndex].AddItem(item.Count);
        }
        else
        {
            // 新しいスロットにアイテムを追加
            _items.Add(item);
        }
        OnGetItem?.Invoke(this);
    }

    public void RemoveItem(ItemStack item)
    {
        // 個数を引く
        Debug.Log("got");
        var findItemIndex = _items.FindIndex(x => x.Item.Name == item.Item.Name);
        if (findItemIndex != -1)
        {
            _items[findItemIndex].RemoveItem(item.Count);
        }
        OnGetItem?.Invoke(this);
    }
}