using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public ItemStack[] Stack => _items;
    ItemStack[] _items = new ItemStack[15];

    public event Action<Inventory> OnGetItem;
    public event Action<ItemStack> OnOverItem;

    public Inventory()
    {
        for (int i = 0; i < _items.Length; i++)
            _items[i] = new(null, 0);
    }

    public void AddItem(ItemStack stack)
    {
        if (stack.Count == 0) return;

        // 空のスロット、取得アイテムと同名のアイテムを保持するスロットを検索
        List<int> EmptySlotIndex = new(), FindItemSlotIndex = new();
        for (int i = 0; i < 5; i++)
        {
            // インデックスの取得
            if (_items[i].Item == null) EmptySlotIndex.Add(i);
            else if (_items[i].Item.Name == stack.Item.Name) FindItemSlotIndex.Add(i);
        }

        // 取得アイテムと同名のアイテムを保持するスロットで検索をかける
        for (int i = 0; i < FindItemSlotIndex.Count; i++)
        {
            var slot = _items[FindItemSlotIndex[i]];

            // 中断処理
            if (slot.Count == slot.Item.MaxCount) continue;

            // 合計が、スタック可能アイテム数より少なければ、
            if (slot.Count + stack.Count < slot.Item.MaxCount)
            {
                // アイテムを増加
                slot.AddItem(stack.Count);
                OnGetItem?.Invoke(this);
                Debug.Log("return");
                return;
            }
            else
            {
                // アイテムを減少
                Debug.Log(slot.Count + stack.Item.MaxCount + " / " +  slot.Item.MaxCount);
                var addCount = slot.Item.MaxCount - slot.Count;
                slot.AddItem(addCount);
                stack.RemoveItem(addCount);
                Debug.Log("not return");
            }
        }

        // 空のスロットで検索をかける
        for (int i = 0; i < EmptySlotIndex.Count; i++)
        {
            var slot = _items[EmptySlotIndex[i]];

            // 合計が、スタック可能アイテム数より少なければ、
            if (stack.Count <= stack.Item.MaxCount)
            {
                // アイテムを増加
                _items[EmptySlotIndex[i]] = new(stack.Item, stack.Count);
                OnGetItem?.Invoke(this);
                return;
            }
            else
            {
                // アイテムを取得
                _items[EmptySlotIndex[i]] = new(stack.Item, stack.Item.MaxCount);
                stack.RemoveItem(stack.Item.MaxCount);
            }
        }

        // アイテムが溢れる場合
        OnOverItem?.Invoke(stack);
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
        OnGetItem?.Invoke(this);
    }
}