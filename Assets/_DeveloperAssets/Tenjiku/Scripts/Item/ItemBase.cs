using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item")]
public class Item : ScriptableObject
{
    // --- プロパティ ---
    public string Name => _name;
    public string Lore => _lore;
    public ItemId Id => _id;
    public Sprite Sprite => _sprite;
    public Material Material => _material;
    public int MaxCount => _maxCount;
    public int Value => _value;
    //public List<ItemType> Type => _type;
    public bool Once => _once;
    public List<ItemAbility> ItemAbility => _itemAbility;
    public float Cooltime => _cooltime;
    public bool CanMix => _canMix;

    // --- メンバ変数 ---
    [Header("▼ 概要")]
    [SerializeField] protected ItemId _id;
    [SerializeField] protected string _name;  //名前
    [SerializeField] protected string _lore;  //説明
    [SerializeField] protected Sprite _sprite; //画像
    [SerializeField] protected Material _material; //画像
    [SerializeField] protected int _maxCount = int.MaxValue; //最大数
    [SerializeField] protected int _value;    //売値
    [SerializeField] protected float _cooltime = 0.0f;
    //[SerializeField] protected List<ItemType> _type;

    [Header("▼ 効能")]
    [SerializeField] protected bool _once = false;
    [SerializeField] protected bool _canMix = true;
    [SerializeField] protected List<ItemAbility> _itemAbility;  //インタラクト
}

[Serializable]
public class ItemStack
{
    // --- プロパティ ---
    public Item Item => _item;
    public int Count => _count;
    public List<ItemAbility> Ability => _ability;
    public virtual Sprite Sprite => _sprite;

    // --- メンバ変数 ---
    [SerializeField] protected Item _item;
    [SerializeField] protected int _count = 1;
    protected List<ItemAbility> _ability;
    protected Sprite _sprite;

    // --- コンストラクタ ---
    public ItemStack(Item item, int count = 1)
    {
        if (item == null) return;

        _item = item;
        _count = count;

        // 画像の設定
        _sprite = Item.Sprite;

        // アビリティがあれば取得
        if (item.ItemAbility != null)
            _ability = new(item.ItemAbility);
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

    public void AddAbility(ItemAbility assets)
    {
        _ability.Add(assets);
    }
}

public enum ItemType
{
    Tool,
    Potion
}