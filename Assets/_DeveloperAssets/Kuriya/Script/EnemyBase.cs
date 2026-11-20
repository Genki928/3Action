using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] protected List<DropItem> _dropItem = new();
    [SerializeField] protected GameObject _deathEffect; // 倒したときに出すエフェクト(煙など。未設定なら出さない)

    // --- 共通フィールド ---
    protected CharacterBase _target;
    protected Vector3 _currentDir;
    AnimControl _animControl; // アニメーションを持つ敵だけ使う

    [Header("▼ Phisics")]
    [SerializeField] protected float MOVE_SPEED = 1.0f;
    [SerializeField] protected float KNOCKBACK_POWER = 5.0f;
    [SerializeField] protected float KNOCKBACK_DURATION = 0.2f;

    [Header("▼ Status")]
    [SerializeField] protected int MAX_HP = 3;
    [SerializeField] protected int POWER = 1;

    [Header("▼ Facing")]
    [SerializeField] protected float FACING_DEADZONE = 0.1f; // これより横成分が小さいときは向きを変えない
    [SerializeField] protected float FACING_HOLD_TIME = 0.3f; // 向きを変えたあと、この秒数は再び変えない(チラつき防止)

    int _facingSign;          // 今の向き(-1 / 0:未設定 / 1)
    float _lastFlipTime;      // 最後に向きを変えた時刻

    [Header("▼ Avoid")]
    [SerializeField] protected string OBSTACLE_TAG = "object";
    [SerializeField] protected float AVOID_DISTANCE = 3.0f;
    [SerializeField] protected float AVOID_RADIUS = 0.4f;
    [SerializeField] protected float TURN_SPEED = 10.0f;

    [Header("▼ Flee")]
    [SerializeField] protected float FLEE_SPEED_RATE = 1.0f; // 昼に逃げるときの速度倍率
    [SerializeField] protected float FLEE_FADE_DELAY = 5.0f;    // 逃げ始めてから透明化を始めるまでの時間(秒)
    [SerializeField] protected float FLEE_FADE_DURATION = 2.0f; // 透明になりきって消えるまでの時間(秒)

    LegStateManager _phaseManager;
    float _fleeTimer; // 逃げ続けている時間

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP
    static readonly int ColorId = Shader.PropertyToID("_Color");         // Built-in

    // --- 共通メソッド ---
    protected override void Start()
    {
        base.Start();

        _animControl = GetComponentInChildren<AnimControl>();

        // HPを持たせえる
        _health = new Health(MAX_HP, MAX_HP);

        // ダメージを受けたとき、HPが0なら倒す
        _health.OnDamage += OnDamaged;

        _target = FindAnyObjectByType<PlayerController>();
        _phaseManager = FindAnyObjectByType<LegStateManager>();
    }

    // 昼は逃げる。RaidState以外を昼とみなしている(昼夜の管理に合わせて書き換える)。
    // 敵ごとに変えたい場合はオーバーライドする
    protected virtual bool IsFleeing =>
        _phaseManager != null && _phaseManager.State is not RaidState;

    // 逃げ続けた時間に応じて徐々に透明にして、最後は消す
    void UpdateFleeFade(float dt)
    {
        if (!IsFleeing)
        {
            // 昼が終わったら元に戻す
            if (_fleeTimer > 0.0f)
            {
                _fleeTimer = 0.0f;
                SetAlpha(1.0f);
            }
            return;
        }

        _fleeTimer += dt;

        float fade = (_fleeTimer - FLEE_FADE_DELAY) / Mathf.Max(FLEE_FADE_DURATION, 0.01f);
        if (fade <= 0.0f) return;

        SetAlpha(1.0f - Mathf.Clamp01(fade));

        // 完全に透明になったら消える(ドロップやエフェクトは出さない)
        if (fade >= 1.0f) Destroy(gameObject);
    }

    void SetAlpha(float alpha)
    {
        if (_renderer == null) return;

        Material m = _renderer.material;
        int id = m.HasProperty(BaseColorId) ? BaseColorId : ColorId;
        if (!m.HasProperty(id)) return;

        Color c = m.GetColor(id);
        c.a = alpha;
        m.SetColor(id, c);
    }

    protected virtual void FixedUpdate()
    {
        // 逃走時間の計測と透明化(ノックバック中やターゲットなしでも進める)
        UpdateFleeFade(Time.fixedDeltaTime);

        // ノックバック中は自分の移動処理で速度を上書きしない
        if (_isKnockBack) return;

        if (_target == null)
        {
            OnNoTarget();
            return;
        }

        // プレイヤーの方向
        Vector3 moveVec = _target.transform.position - transform.position;
        moveVec.y = 0.0f;

        // 昼はプレイヤーと反対方向へ
        bool fleeing = IsFleeing;
        if (fleeing) moveVec = -moveVec;

        if (moveVec.sqrMagnitude > 0.0f)
        {
            moveVec.Normalize();
        }

        // 障害物を避ける
        moveVec = AvoidObstacles(moveVec);

        // 向きをなめらかに変える
        if (_currentDir == Vector3.zero) _currentDir = moveVec;
        _currentDir = Vector3.Slerp(_currentDir, moveVec, TURN_SPEED * Time.fixedDeltaTime);

        // 横方向の速度(追従+迂回) → 敵ごとの味付けは派生側で
        float speed = MOVE_SPEED * (fleeing ? FLEE_SPEED_RATE : 1.0f);
        Vector3 vel = ComputeVelocity(_currentDir * speed);
        _rigidbody.linearVelocity = vel;

        FaceMoveDirection();
    }

    // ターゲットがいないときの挙動(デフォルト: 横だけ止める)
    protected virtual void OnNoTarget()
    {
        Vector3 v = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = new Vector3(0, v.y, 0);
    }

    // 最終的な速度を決める(デフォルト: Yは現在の値を残す)
    protected virtual Vector3 ComputeVelocity(Vector3 horizontalVel)
    {
        horizontalVel.y = _rigidbody.linearVelocity.y;
        return horizontalVel;
    }

    // 向く方向のX成分(デフォルト: プレイヤーの方向)。向きを変えたい敵はオーバーライドする
    protected virtual float FacingX => _currentDir.x;

    // 向きに合わせてテクスチャを左右反転
    protected void FaceMoveDirection()
    {
        float x = FacingX;
        if (Mathf.Abs(x) < FACING_DEADZONE) return;

        int sign = x > 0.0f ? 1 : -1;

        // 向きを変えた直後は、一定時間は変えない
        if (sign != _facingSign)
        {
            if (_facingSign != 0 && Time.time - _lastFlipTime < FACING_HOLD_TIME) return;
            _facingSign = sign;
            _lastFlipTime = Time.time;
        }

        float dir = sign;

        // AnimControl がある場合は、そちらが毎フレーム Tiling/Offset を上書きするので flipX で渡す
        if (_animControl != null)
        {
            _animControl.SetFlip(dir > 0.0f);
            return;
        }

        _renderer.material.mainTextureScale =
            new Vector2(-dir, 1);

        _renderer.material.mainTextureOffset =
            new Vector2(-dir < 0 ? 1 : 0, 0);
    }

    void OnDamaged()
    {
        if (_health.Value <= 0)
        {
            OnDefeated();
        }
    }

    // 倒されたときの処理(デフォルト: アイテムを落として消える)
    protected virtual void OnDefeated()
    {
        foreach (var item in _dropItem)
            item.Drop(this);

        // 倒した場所にエフェクトを出す(向きはプレハブ自身の向きを使う)
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, transform.position, _deathEffect.transform.rotation);
        }

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        // イベント購読を解除(念のため)
        if (_health != null) _health.OnDamage -= OnDamaged;
    }

    protected Vector3 AvoidObstacles(Vector3 moveVec)
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

    protected virtual void OnCollisionEnter(Collision collision)
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


[Serializable]
public class DropItem
{
    [SerializeField] int _dropChance = 0;
    [SerializeField] Item _stack;
    [SerializeField] int _count = 0;

    public void Drop(CharacterBase owner)
    {
        // 確率の計算
        if (UnityEngine.Random.Range(1, 101) < _dropChance)
        {
            ItemGenerator.Instance.ItemGenerate(owner.transform.position, new(_stack, _count));
        }
    }
}