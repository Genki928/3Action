using UnityEngine;

public class CharacterBase : ObjectBase
{
    // --- プロパティ
    public Health Health => _health;

    // --- フィールド ---
    protected Rigidbody _rigidbody;
    protected MoveSpeed _moveSpeed;
    protected Health _health;

    // --- メソッド ---
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}