using UnityEngine;

public abstract class InteractAssetEffect : ItemAbility
{
    [SerializeField] protected int _value;
    [SerializeField] protected float _time = 3.0f;
}