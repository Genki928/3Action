using UnityEngine;

public class CharacterBase : ObjectBase
{
    // --- フィールド ---
    protected Rigidbody _rigidbody;
    protected MoveSpeed _moveSpeed;

    // --- メソッド ---
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}