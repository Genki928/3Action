using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] protected List<DropItem> _dropItem = new();
}


[Serializable]
public class DropItem
{
    [SerializeField] int _dropChance = 0;
    [SerializeField] Item _stack;
    [SerializeField] int _count = 0;

    public void Drop(CharacterBase owner)
    {
        // 確率の計算
        if (UnityEngine.Random.Range(1, 101) < _dropChance)
        {
            ItemGenerator.Instance.ItemGenerate(owner.transform.position, new(_stack, _count));
        }
    }
}