using System;
using Unity.Mathematics;
using UnityEngine;

public class Inventory
{
    public ItemStack[] Stack => _items;
    ItemStack[] _items = new ItemStack[15];

    public event Action<Inventory> OnGetItem;

    public Inventory()
    {
        for (int i = 0; i < _items.Length; i++)
            _items[i] = new(null, 0);
    }

    public void AddItem(ItemStack stack)
    {
        // 個数を足す
        var findItemIndex = Array.FindIndex(_items, x => x.Item != null && x.Item.Name == stack.Item.Name);
        if (findItemIndex != -1)
        {
            // アイテムを増加
            _items[findItemIndex].AddItem(stack.Count);
        }
        else
        {
            // 新しいスロットにアイテムを追加
            var emptySlot = Array.FindIndex(_items, x => x.Item == null);
            _items[emptySlot] = new(stack.Item);
        }
        OnGetItem?.Invoke(this);
    }

    public void RemoveItem(int itemIndex, int count)
    {
        // 個数を引く
        _items[itemIndex].RemoveItem((_items[itemIndex].Count - count) < 0 ? _items[itemIndex].Count : count);
        if (_items[itemIndex].Count == 0) _items[itemIndex] = new(null, 0);
        OnGetItem?.Invoke(this);
    }

    public void Swap(int a, int b)
    {
        if (a == b) return;
        (_items[a], _items[b]) = (_items[b], _items[a]);
        OnGetItem?.Invoke(this);   // → Drawが呼ばれて表示が更新される
    }
}