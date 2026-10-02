using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    // --- フィールド ---
    [Header("▼ Camera")]
    [SerializeField] Transform _camera;
    [SerializeField] float DISTANCE_Y, DISTANCE_Z;
    Vector3 _moveVec;

    [Header("▼ Phisics")]
    [SerializeField] float MOVE_SPEED = 5.0f;
    [SerializeField] int START_HEALTH = 3;
    Health _health;

    // --- メソッド ---
    protected override void Start()
    {
        base.Start();
        _moveSpeed = new(MOVE_SPEED);
        _health = new(START_HEALTH);
    }

    void LateUpdate()
    {
        _camera.position = new(transform.position.x, transform.position.y + DISTANCE_Y, transform.position.z + DISTANCE_Z);
    }

    void FixedUpdate()
    {
        // ベクトルの補正
        var vec = _isPaused ? 0.0f : _moveSpeed.Value;
        GetComponent<Rigidbody>().linearVelocity = _moveVec * vec;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        // 移動ベクトルの保存
        var vec = ctx.ReadValue<Vector2>();
        _moveVec = new(vec.x, 0.0f, vec.y);

    }
}