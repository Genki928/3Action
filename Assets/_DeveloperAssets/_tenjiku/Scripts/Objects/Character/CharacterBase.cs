using UnityEngine;

public class CharacterBase : ObjectBase
{
    // --- フィールド ---
    protected Rigidbody2D _rigidbody;

    // --- メソッド ---
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}