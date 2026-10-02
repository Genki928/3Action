using UnityEngine;

public class CharacterBase : ObjectBase
{
    // --- フィールド ---
    protected Rigidbody _rigidbody;

    // --- メソッド ---
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}