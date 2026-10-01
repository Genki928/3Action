using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    // --- フィールド ---
    Vector2 _moveVec;

    // --- メソッド ---
    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        var vec = _isPaused ? 0.0f : 5.0f;
        GetComponent<Rigidbody>().linearVelocity = _moveVec * vec;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _moveVec = ctx.ReadValue<Vector2>();
    }
}