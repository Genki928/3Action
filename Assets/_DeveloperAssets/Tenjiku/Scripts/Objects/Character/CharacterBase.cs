using UnityEngine;

public class CharacterBase : ObjectBase
{
    // --- プロパティ --- //
    public Health Health => _health;
    public MoveSpeed MoveSpeed => _moveSpeed;
    public Power Power => _power;

    // --- フィールド --- //
    [SerializeField] protected ItemObj _itemPrefab;
    protected Rigidbody _rigidbody;
    protected MoveSpeed _moveSpeed = new(1);
    protected Health _health = new(1);
    protected Power _power = new(1);

    protected Renderer _renderer;

    /// <summary>
    /// ノックバック中かどうか
    /// </summary>
    protected bool _isKnockBack;

    // --- メソッド --- //
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    protected virtual void Update()
    {
        _moveSpeed.EffectUpdate(Time.deltaTime);
        _power.EffectUpdate(Time.deltaTime);
    }

    /// <summary>
    /// キャラクターをノックバックさせる
    /// </summary>
    /// <param name="direction">ノックバックさせる方向</param>
    /// <param name="power">ノックバックの強さ</param>
    /// <param name="duration">ノックバックする時間</param>
    public void KnockBack(Vector3 direction, float power, float duration)
    {
        direction.y = 0.0f;
        direction.Normalize();

        _isKnockBack = true;

        // 現在の速度をリセット
        _rigidbody.linearVelocity = Vector3.zero;

        // ノックバック
        _rigidbody.AddForce(direction * power, ForceMode.Impulse);

        // 一定時間後にノックバック解除
        Invoke(nameof(EndKnockBack), duration);
    }

    /// <summary>
    /// ノックバックを終了するﾜﾖ
    /// </summary>
    void EndKnockBack()
    {
        _isKnockBack = false;
    }
}