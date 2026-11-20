using UnityEngine;

// 蝙蝠: ちょこまか不規則に近づく → 急降下で突っ込む → ぶつかったら(または外したら)上空へ逃げる、を繰り返す
public class BAT : EnemyBase
{
    enum State { Flutter, Dive, Retreat }

    [Header("▼ HuyoHuyo")]
    [SerializeField] float HOVER_HEIGHT = 5.0f;    // 飛んでいる高さの中心
    [SerializeField] float FLOAT_AMPLITUDE = 1.0f; // 上下の揺れ幅
    [SerializeField] float FLOAT_SPEED = 3.0f;     // 上下に揺れる速さ
    [SerializeField] float HEIGHT_FOLLOW = 5.0f;   // 高さへの追従の強さ

    [Header("▼ Flutter(ちょこまか接近)")]
    [SerializeField] Vector2 BURST_INTERVAL = new(0.25f, 0.7f);               // 速度を切り替える間隔(秒)
    [SerializeField] Vector2 SPEED_MULTIPLIER_RANGE = new(0.3f, 2.0f);        // 速度のムラ(MOVE_SPEEDに掛ける)

    [Header("▼ Dive(急降下)")]
    [SerializeField] float DIVE_TRIGGER_RANGE = 10.0f; // この距離(水平)まで近づいたら突っ込む
    [SerializeField] float DIVE_SPEED = 10.0f;        // 突進の速さ
    [SerializeField] float DIVE_MAX_TIME = 10.0f;      // 突進の最大時間
    [SerializeField] float DIVE_MIN_HEIGHT = 1.0f;    // 突進で下がれる最低の高さ(プレイヤーの足元からの高さ。地面に当たるなら上げる)
    [SerializeField] Vector2 DIVE_COOLDOWN = new(1.0f, 2.5f); // 次の突進までの待ち時間(秒)

    [Header("▼ Retreat(通り抜けて弧を描いて上昇)")]
    [SerializeField] float RETREAT_SPEED = 2.5f;             // 弧の終わりの速さ(突進速度から徐々に落ちる)
    [SerializeField] float RETREAT_RISE_ANGLE = 50.0f;       // 弧の終わりの上向き角度(度)
    [SerializeField] Vector2 RETREAT_TIME = new(0.8f, 1.2f); // 弧を描く時間(秒)。長いほど大きな弧になる

    [Header("▼ Reflect(突進中・通り抜け中にダメージを受けたとき)")]
    [SerializeField] float REFLECT_SPEED_RATIO = 0.6f; // 跳ね返される初速(DIVE_SPEEDに対する割合)

    State _state = State.Flutter;
    float _stateTimer;
    float _cooldown;
    float _burstTimer;
    float _speedMul = 1.0f;  // 速度のムラ
    float _floatOffset;      // 個体ごとに揺れをずらす
    Vector3 _diveTarget;
    float _floorY;          // 突進・通り抜け中に、これより下へは沈まない高さ
    Vector3 _diveDir;       // 突進中の進行方向(通り抜けに引き継ぐ)
    Vector3 _retreatDir;    // 通り抜け中の水平の進行方向(向きの表示にも使う)
    float _retreatDuration; // 今回の弧の長さ(秒)
    float _startPitch;      // 弧の開始時の上下角(ラジアン。突進中は下向きでマイナス)
    float _retreatStartSpeed; // 弧の開始時の速さ

    protected override void Start()
    {
        base.Start();

        // Rigidbodyの重力を切る(浮かせるため)
        _rigidbody.useGravity = false;

        _floatOffset = Random.value * Mathf.PI * 2.0f;
        _cooldown = Random.Range(DIVE_COOLDOWN.x, DIVE_COOLDOWN.y);

        // ダメージを受けたときの反射(HPは base.Start() で作られる)
        _health.OnDamage += OnDamagedReflect;
    }

    protected override void OnDestroy()
    {
        if (_health != null) _health.OnDamage -= OnDamagedReflect;
        base.OnDestroy();
    }

    // 突進中・通り抜け中にダメージを受けたら、来た方向へ跳ね返されて上昇する
    void OnDamagedReflect()
    {
        if (_state == State.Dive)
        {
            // 水平は逆向き、縦は上向きに反射
            _diveDir = new Vector3(-_diveDir.x, Mathf.Abs(_diveDir.y), -_diveDir.z).normalized;
        }
        else if (_state == State.Retreat)
        {
            // 通り抜けの進行方向を逆向きにして、少し上へ
            _diveDir = (-_retreatDir + Vector3.up * 0.5f).normalized;
        }
        else
        {
            return;
        }

        StartRetreat(DIVE_SPEED * REFLECT_SPEED_RATIO);
    }

    // 目標の高さ(ふよふよ)
    float GetTargetY()
    {
        return HOVER_HEIGHT
            + Mathf.Sin(Time.time * FLOAT_SPEED + _floatOffset) * FLOAT_AMPLITUDE;
    }

    // ターゲットがいないときは、その場で浮くだけ
    protected override void OnNoTarget()
    {
        _rigidbody.linearVelocity =
            new Vector3(0, (GetTargetY() - transform.position.y) * HEIGHT_FOLLOW, 0);
    }

    protected override Vector3 ComputeVelocity(Vector3 vel)
    {
        float dt = Time.fixedDeltaTime;

        // 昼は突進をやめて逃げる(突進中なら中断して通常の飛行に戻す)
        if (IsFleeing && _state == State.Dive)
        {
            _state = State.Flutter;
            _cooldown = Random.Range(DIVE_COOLDOWN.x, DIVE_COOLDOWN.y);
        }

        Vector3 flat = _target.transform.position - transform.position;
        flat.y = 0.0f;

        switch (_state)
        {
            case State.Dive: return DiveVelocity(flat, dt);
            case State.Retreat: return RetreatVelocity(dt);
            default: return FlutterVelocity(vel, flat, dt);
        }
    }

    // ちょこまか接近: 速度のムラを不規則に切り替える
    Vector3 FlutterVelocity(Vector3 vel, Vector3 flat, float dt)
    {
        UpdateBurst(dt);
        _cooldown -= dt;

        vel *= _speedMul;
        vel.y = (GetTargetY() - transform.position.y) * HEIGHT_FOLLOW;

        // 近づいたら突っ込む
        if (_cooldown <= 0.0f && flat.magnitude < DIVE_TRIGGER_RANGE)
        {
            _state = State.Dive;
            _stateTimer = DIVE_MAX_TIME;
            // 突進開始時のプレイヤー位置を狙う(動けば避けられる)
            _diveTarget = _target.transform.position + Vector3.up * DIVE_MIN_HEIGHT;
            _floorY = _diveTarget.y;
            _diveDir = (_diveTarget - transform.position).normalized;
        }

        return vel;
    }

    // 急降下: ロックした位置へ一直線
    Vector3 DiveVelocity(Vector3 flat, float dt)
    {
        _stateTimer -= dt;

        Vector3 toDive = _diveTarget - transform.position;
        if (_stateTimer <= 0.0f || toDive.magnitude < 0.5f)
        {
            StartRetreat();
            return RetreatVelocity(dt);
        }

        _diveDir = toDive.normalized;
        return ClampFloor(_diveDir * DIVE_SPEED);
    }

    // 通り抜け: 突進の勢いのまま進み、徐々に減速しながら上向きに曲がって弧を描く
    Vector3 RetreatVelocity(float dt)
    {
        _stateTimer -= dt;

        float t = 1.0f - Mathf.Clamp01(_stateTimer / _retreatDuration);
        float eased = t * t * (3.0f - 2.0f * t); // なめらかに変化させる

        float pitch = Mathf.Lerp(_startPitch, RETREAT_RISE_ANGLE * Mathf.Deg2Rad, eased);
        float speed = Mathf.Lerp(_retreatStartSpeed, RETREAT_SPEED, eased);

        Vector3 vel = (_retreatDir * Mathf.Cos(pitch) + Vector3.up * Mathf.Sin(pitch)) * speed;

        if (_stateTimer <= 0.0f)
        {
            _state = State.Flutter;
            _cooldown = Random.Range(DIVE_COOLDOWN.x, DIVE_COOLDOWN.y);
        }

        return ClampFloor(vel);
    }

    // 最低の高さまで下がったら、それ以上は沈まないようにする(地面に引っかかるのを防ぐ)
    Vector3 ClampFloor(Vector3 vel)
    {
        if (transform.position.y <= _floorY && vel.y < 0.0f) vel.y = 0.0f;
        return vel;
    }

    void StartRetreat() => StartRetreat(DIVE_SPEED);

    void StartRetreat(float startSpeed)
    {
        _state = State.Retreat;
        _retreatStartSpeed = startSpeed;
        _retreatDuration = Random.Range(RETREAT_TIME.x, RETREAT_TIME.y);
        _stateTimer = _retreatDuration;

        // 突進していた向きをそのまま引き継ぐ(プレイヤーに当たっても折り返さない)
        Vector3 flatDir = new Vector3(_diveDir.x, 0.0f, _diveDir.z);
        float flatMag = flatDir.magnitude;
        if (flatMag > 0.001f)
        {
            _retreatDir = flatDir / flatMag;
        }
        else
        {
            Vector3 fallback = new Vector3(_currentDir.x, 0.0f, _currentDir.z);
            _retreatDir = fallback.sqrMagnitude > 0.0f ? fallback.normalized : Vector3.right;
        }
        _startPitch = Mathf.Atan2(_diveDir.y, flatMag);
    }

    // 一定間隔ごとに、速度のムラをランダムに切り替える
    void UpdateBurst(float dt)
    {
        _burstTimer -= dt;
        if (_burstTimer > 0.0f) return;

        _burstTimer = Random.Range(BURST_INTERVAL.x, BURST_INTERVAL.y);
        _speedMul = Random.Range(SPEED_MULTIPLIER_RANGE.x, SPEED_MULTIPLIER_RANGE.y);
    }

    // 通り抜け中は進行方向を向く(それ以外はプレイヤーの方向)
    protected override float FacingX => _state == State.Retreat ? _retreatDir.x : base.FacingX;

    // 突進中にプレイヤーか障害物へぶつかったら通り抜けの弧に入る
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (_state != State.Dive) return;

        // 地面との接触では切り替えない(プレイヤーか障害物に当たったときだけ)
        bool hitPlayer = collision.gameObject.GetComponentInParent<PlayerController>() != null;
        bool hitObstacle = collision.collider.CompareTag(OBSTACLE_TAG);
        if (hitPlayer || hitObstacle) StartRetreat();
    }
}