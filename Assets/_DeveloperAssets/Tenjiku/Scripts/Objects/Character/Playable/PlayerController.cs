using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    // --- フィールド ---
    [Header("▼ Camera")]
    [SerializeField] Transform _camera;
    [SerializeField] float distantY, distantZ;
    Vector3 _moveVec;

    // --- メソッド ---
    protected override void Start()
    {
        base.Start();
    }

    void LateUpdate()
    {
        _camera.position = new(transform.position.x, transform.position.y + distantY, transform.position.z + distantZ);
    }

    void FixedUpdate()
    {
        var vec = _isPaused ? 0.0f : 5.0f;
        GetComponent<Rigidbody>().linearVelocity = _moveVec * vec;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        var vec = ctx.ReadValue<Vector2>();
        _moveVec = new(vec.x, 0.0f, vec.y);

    }
}