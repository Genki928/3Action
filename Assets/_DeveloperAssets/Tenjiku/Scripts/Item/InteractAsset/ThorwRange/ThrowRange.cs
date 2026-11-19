using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/ThrowRange")]
public class AbilityAssetThrowRange : ItemAbility
{
    public Sprite Sprite => _potionSprites;
    public float ThrowRange => _throwRange;
    [SerializeField] float _throwRange = 1.0f;
    [SerializeField] Sprite _potionSprites;
}