using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/Damage")]
public class AbilityAssetThrowRange : ItemAbility
{
    public Sprite Sprite => _potionSprites;
    [SerializeField] float _throwRange = 1.0f;
    [SerializeField] Sprite _potionSprites;
}