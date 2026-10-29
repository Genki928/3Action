using UnityEngine;

public class Sword : DamageCollisionBase
{
    [Header("回転設定")]
    [SerializeField] float _rotateAngle = 180.0f;
    [SerializeField] float _rotateSpeed = 720.0f;
    [SerializeField] float _radius = 1.0f;

    [Header("ノックバック設定")]
    [SerializeField] float _knockBackPower = 5.0f;
    [SerializeField] float _knockBackDuration = 0.2f;

    Transform _player;
    PlayerController _playerController;
    Vector3 _attackDirection;

    float _currentAngle;
    bool _isAttacking;

    PlayerController _controller;

    public void Init(PlayerController player, Vector3 direction)
    {
        _playerController = player;
        _player = player.transform;
        _attackDirection = direction.normalized;

        _currentAngle = 0.0f;
        _isAttacking = true;
    }
    protected new void Start()
    {
        base.Start();

        _damage = _playerController.AttackPower;
        Debug.Log(_damage);
    }

    void Update()
    {
        if (!_isAttacking || _player == null || _isPaused)
            return;

        // 右から左へ回転
        _currentAngle += _rotateSpeed * Time.deltaTime;

        float angle = Mathf.Min(_currentAngle, _rotateAngle);

        Vector3 rotatedDirection =
            Quaternion.AngleAxis(
                angle - _rotateAngle / 2.0f,
                Vector3.up
            ) * _attackDirection;

        // プレイヤーを中心に配置
        transform.position =
            _player.position + rotatedDirection * _radius;

        transform.rotation =
     Quaternion.LookRotation(rotatedDirection)
     * Quaternion.Euler(0.0f, 180.0f, 0.0f);

        // 回転終了
        if (_currentAngle >= _rotateAngle)
        {
            _isAttacking = false;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log($"Sword hit: {col.name}");

        if (!_isAttacking || _player == null)
            return;

        // プレイヤー自身には当たらない
        if (col.transform.root == _player.root)
            return;

        var enemy = col.GetComponentInParent<EnemyBase>();

        if (enemy == null)
        {
            Debug.Log("EnemyBase が見つかりません");
            return;
        }

        Debug.Log("敵に攻撃が命中！");

        enemy.Health.Damage(_damage);

        Vector3 knockBackDirection =
            enemy.transform.position - _player.position;

        knockBackDirection.y = 0.0f;

        if (knockBackDirection.sqrMagnitude > 0.0f)
        {
            enemy.KnockBack(
                knockBackDirection,
                _knockBackPower,
                _knockBackDuration
            );
        }
    }
}