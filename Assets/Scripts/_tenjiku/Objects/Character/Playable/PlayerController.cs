using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    Vector2 moveVec;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        var vec = isPaused ? 0.0f : 5.0f;
        rigidbody.linearVelocity = moveVec * vec;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveVec = ctx.ReadValue<Vector2>();
    }
}