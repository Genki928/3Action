using UnityEngine;

public class Sword : DamageCollisionBase
{
    [Header("回転設定")]
    [SerializeField] float _rotateAngle = 180.0f;
    [SerializeField] float _rotateSpeed = 720.0f;
    [SerializeField] float _radius = 1.0f;

    [Header("見た目の設定")]
    [SerializeField] bool _mirrorRightToLeft = false; // OFF: 左から右のときに左右反転 / ON: 右から左のときに反転

    [Header("ノックバック設定")]
    [SerializeField] float _knockBackPower = 5.0f;
    [SerializeField] float _knockBackDuration = 0.2f;

    // 次に振る向き(振るたびに反転する。全ての剣で共有)
    static bool s_reverseNext;

    Transform _player;
    PlayerController _playerController;
    Vector3 _attackDirection;

    float _currentAngle;
    float _swingSign = 1.0f; // 1: 右から左 / -1: 左から右
    bool _isAttacking;

    PlayerController _controller;

    public void Init(PlayerController player, Vector3 direction)
    {
        _playerController = player;
        _player = player.transform;
        _attackDirection = direction.normalized;

        // 振るたびに向きを交互に切り替える
        _swingSign = s_reverseNext ? -1.0f : 1.0f;
        s_reverseNext = !s_reverseNext;

        // 剣の絵は片方の向きに合わせて作ってあるので、逆に振るときは左右反転して見た目を合わせる
        bool mirror = (_swingSign > 0.0f) == _mirrorRightToLeft;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (mirror ? -1.0f : 1.0f);
        transform.localScale = scale;

        _currentAngle = 0.0f;
        _isAttacking = true;
    }
    protected new void Start()
    {
        base.Start();

        _damage = _playerController.AttackPower;
    }

    void Update()
    {
        if (!_isAttacking || _player == null || _isPaused)
            return;

        // 回転を進める
        _currentAngle += _rotateSpeed * Time.deltaTime;

        float angle = Mathf.Min(_currentAngle, _rotateAngle);

        // _swingSign が 1 なら右から左、-1 なら左から右
        Vector3 rotatedDirection =
            Quaternion.AngleAxis(
                _swingSign * (angle - _rotateAngle / 2.0f),
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