using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemBase")]
public class Item : ScriptableObject
{
    // --- プロパティ ---
    public Sprite Sprite => _sprite;
    public string Name => _name;
    public string Lore => _lore;
    public int MaxCount => _maxCount;
    public int Value => _value;
    public List<Effect> Effect => _effect;
    public InteractAsset InteractAsset => _interactAsset;

    // --- メンバ変数 ---
    [SerializeField] Sprite _sprite; //画像
    [SerializeField] string _name;  //名前
    [SerializeField] string _lore;  //説明
    [SerializeField] int _maxCount; //最大数
    [SerializeField] int _value;    //売値
    [SerializeField] List<Effect> _effect; //エフェクト
    [SerializeField] InteractAsset _interactAsset;  //インタラクト
}

[CreateAssetMenu(menuName = "Item/InteractAsset")]
public abstract class InteractAsset : ScriptableObject
{
    public abstract void Interact();
}

[Serializable]
public class ItemStack
{
    // --- プロパティ ---
    public Item Item => _item;
    public int Count => _count;

    // --- メンバ変数 ---
    [SerializeField] Item _item;
    [SerializeField] int _count;

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
    public bool AddItem(int add)
    {
        _count = Math.Min(_count - add, _item.MaxCount);
        return _count == _item.MaxCount;
    }
}

[Serializable]
public class Effect
{
    [SerializeField] EffectType _effectType;
    [SerializeField] float _time;
    [SerializeField] float _value;
}

public enum EffectType
{
    MoveSpeed,
    Power,
}