using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemBase")]
public class Item : ScriptableObject
{
    // --- プロパティ ---
    public Sprite Sprite => _sprite;
    public Material Material => _material;
    public string Name => _name;
    public string Lore => _lore;
    public int MaxCount => _maxCount;
    public int Value => _value;
    public List<ItemType> Type => _type;
    public bool Once => _once;
    public List<Effect> Effect => _effect;
    public List<InteractAsset> InteractAsset => _interactAsset;

    // --- メンバ変数 ---
    [Header("▼ 概要")]
    [SerializeField] Sprite _sprite; //画像
    [SerializeField] Material _material; //画像
    [SerializeField] string _name;  //名前
    [SerializeField] string _lore;  //説明
    [SerializeField] int _maxCount = int.MaxValue; //最大数
    [SerializeField] int _value;    //売値
    [SerializeField] List<ItemType> _type;

    [Header("▼ 効能")]
    [SerializeField] bool _once = false;
    [SerializeField] List<Effect> _effect; //エフェクト
    [SerializeField] List<InteractAsset> _interactAsset;  //インタラクト
}

[Serializable]
public class ItemStack
{
    // --- プロパティ ---
    public Item Item => _item;
    public int Count => _count;
    public List<InteractAsset> ClickEvent => _interactAsset;

    // --- メンバ変数 ---
    [SerializeField] Item _item;
    [SerializeField] int _count = 1;
    List<InteractAsset> _interactAsset;

    // --- コンストラクタ ---
    public ItemStack(Item item, int count = 1)
    {
        _item = item;
        _count = count;

        if (item != null && item.InteractAsset != null)
            _interactAsset = new(item.InteractAsset);
    }

    // --- メソッド ---
    /// <summary> アイテムの個数減少 </summary>
    /// <param name="remove"> 減らす数 </param>
    /// <returns> 0か否か </returns>
    public bool RemoveItem(int remove)
    {
        _count = Math.Max(0, _count - remove);
        return _count == 0;
    }

    /// <summary> アイテムの個数増加 </summary>
    /// <param name="remove"> 増やす数 </param>
    /// <returns> 最大か否か </returns>
    public void AddItem(int add)
    {
        //_count = Math.Min(_count + add, _item.MaxCount);
        //return _count == _item.MaxCount;
        _count = _count + add;
    }

    public void AddClickEvent(InteractAsset assets)
    {
        _interactAsset.Add(assets);
    }
}

public enum ItemType
{
    Tool,
    Potion
}