using UnityEngine;

public class Ghost : CharacterBase
{
    // --- フィールド ---
    CharacterBase _target;

    [Header("▼ Phisics")]
    [SerializeField] float MOVE_SPEED = 1.0f;
    [SerializeField] float KNOCKBACK_POWER = 5.0f;
    [SerializeField] float KNOCKBACK_DURATION = 0.2f;

    [Header("▼ Status")]
    [SerializeField] int MAX_HP = 3;
    [SerializeField] int POWER = 1;

    [Header("▼ Avoid")]
    [SerializeField] string OBSTACLE_TAG = "object";
    [SerializeField] float AVOID_DISTANCE = 3.0f;
    [SerializeField] float AVOID_RADIUS = 0.4f;
    [SerializeField] float TURN_SPEED = 10.0f;

    Vector3 _currentDir;

    [Header("▼ HuyoHuyo")]
    [SerializeField] float HOVER_HEIGHT = 10.0f;   // 浮く高さの中心
    [SerializeField] float FLOAT_AMPLITUDE = 0.3f; // 上下の揺れ幅
    [SerializeField] float FLOAT_SPEED = 2.0f;     // 揺れる速さ
    [SerializeField] float SWAY_AMOUNT = 0.3f;     // 横の揺らぎの強さ
    [SerializeField] float HEIGHT_FOLLOW = 5.0f;   // 高さへの追従の強さ

    float _floatOffset; // ゴーストごとに揺れをずらす

    // --- メソッド ---
    protected override void Start()
    {
        base.Start();

        // HPを作る(親の _health に入れる)
        _health = new Health(MAX_HP, MAX_HP);

        // ダメージを受けたとき、HPが0なら倒す
        _health.OnDamage += OnDamaged;

        // Rigidbodyの重力を切る(ふわふわ浮かせるため)
        _rigidbody.useGravity = false;

        // 複数体いても揺れがバラけるように、ランダムな位相を持たせる
        _floatOffset = Random.value * Mathf.PI * 2.0f;

        _target = FindAnyObjectByType<PlayerController>();
    }


    void FixedUpdate()
    {
        // ノックバック中は自分の移動処理で速度を上書きしない
        if (_isKnockBack) return;

        // 目標の高さ(ふよふよ)。ターゲットの有無に関わらず使う
        float targetY = HOVER_HEIGHT
            + Mathf.Sin(Time.time * FLOAT_SPEED + _floatOffset) * FLOAT_AMPLITUDE;

        // ターゲットがいないときは、その場でふよふよだけする
        if (_target == null)
        {
            _rigidbody.linearVelocity =
                new Vector3(0, (targetY - transform.position.y) * HEIGHT_FOLLOW, 0);
            return;
        }

        // プレイヤーの方向
        Vector3 moveVec = _target.transform.position - transform.position;
        moveVec.y = 0.0f;

        if (moveVec.sqrMagnitude > 0.0f)
        {
            moveVec.Normalize();
        }

        // 障害物を避ける
        moveVec = AvoidObstacles(moveVec);

        // 向きをなめらかに変える
        if (_currentDir == Vector3.zero) _currentDir = moveVec;
        _currentDir = Vector3.Slerp(_currentDir, moveVec, TURN_SPEED * Time.fixedDeltaTime);

        // 横方向の速度(追従+迂回)
        Vector3 vel = _currentDir * MOVE_SPEED;

        // 横の揺らぎ: 進行方向に対して左右にゆらゆらさせる
        Vector3 side = Vector3.Cross(Vector3.up, _currentDir);
        float sway = Mathf.Sin(Time.time * FLOAT_SPEED * 0.7f + _floatOffset);
        vel += side * sway * SWAY_AMOUNT;

        // 縦方向: 目標の高さに向かって速度を出す
        vel.y = (targetY - transform.position.y) * HEIGHT_FOLLOW;

        _rigidbody.linearVelocity = vel;

        // プレイヤーの方向を向く(テクスチャ左右反転)
        if (_currentDir.x != 0.0f)
        {
            float dir = Mathf.Sign(_currentDir.x);

            _renderer.material.mainTextureScale =
                new Vector2(-dir, 1);

            _renderer.material.mainTextureOffset =
                new Vector2(-dir < 0 ? 1 : 0, 0);
        }
    }
    void OnDamaged()
    {
        if (_health.Value <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // イベント購読を解除(念のため)
        if (_health != null) _health.OnDamage -= OnDamaged;
    }
    Vector3 AvoidObstacles(Vector3 moveVec)
    {
        if (moveVec == Vector3.zero) return moveVec;

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(
            origin, AVOID_RADIUS, moveVec, AVOID_DISTANCE);

        RaycastHit nearest = default;
        float nearestDist = float.MaxValue;
        bool found = false;

        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag(OBSTACLE_TAG)) continue;
            if (hit.distance < nearestDist)
            {
                nearestDist = hit.distance;
                nearest = hit;
                found = true;
            }
        }

        if (!found) return moveVec;

        Vector3 side = Vector3.Cross(Vector3.up, moveVec);

        Vector3 toMe = transform.position - nearest.collider.bounds.center;
        toMe.y = 0.0f;
        if (Vector3.Dot(side, toMe) < 0.0f) side = -side;

        float t = 1.0f - Mathf.Clamp01(nearestDist / AVOID_DISTANCE);
        t = Mathf.Clamp01(t * t * 2.0f);

        return Vector3.Lerp(moveVec, side, t).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーにだけ当てる(敵同士でダメージが入らないように)
        PlayerController player =
            collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Health.Damage(POWER);

            Vector3 knockBackDirection =
                player.transform.position - transform.position;

            player.KnockBack(knockBackDirection, KNOCKBACK_POWER, KNOCKBACK_DURATION);
        }
    }
}