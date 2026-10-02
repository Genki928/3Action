using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    // --- フィールド ---
    CharacterBase _target;

    [Header("▼ Phisics")]
    [SerializeField] float MOVE_SPEED = 1.0f;
    [SerializeField] int DAMAGE = 1;
    [SerializeField] float KNOCKBACK_POWER = 5.0f;
    [SerializeField] float KNOCKBACK_DURATION = 0.2f;


    Rigidbody _rigidbody;
    Renderer _renderer;

    // --- メソッド ---
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();

        // Playerを取得
        _target = FindAnyObjectByType<PlayerController>();
    }

    void FixedUpdate()
    {
        if (_target == null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            return;
        }

        // プレイヤーの方向
        Vector3 moveVec = _target.transform.position - transform.position;

        // Y方向は無視
        moveVec.y = 0.0f;

        // 正規化
        if (moveVec.sqrMagnitude > 0.0f)
        {
            moveVec.Normalize();
        }

        // 移動
        _rigidbody.linearVelocity = moveVec * MOVE_SPEED;

        // プレイヤーの方向を向く
        if (moveVec.x != 0.0f)
        {
            float dir = Mathf.Sign(moveVec.x);

            _renderer.material.mainTextureScale =
                new Vector2(-dir, 1);

            _renderer.material.mainTextureOffset =
                new Vector2(-dir < 0 ? 1 : 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CharacterBase character =
            collision.gameObject.GetComponentInParent<CharacterBase>();

        if (character != null)
        {
            character.Health.Damage(DAMAGE);

            Vector3 knockBackDirection =
                character.transform.position - transform.position;

            character.KnockBack(knockBackDirection,KNOCKBACK_POWER,KNOCKBACK_DURATION);
        }
    }
}