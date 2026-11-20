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

    [Header("▼ Avoid")]
    [SerializeField] protected string OBSTACLE_TAG = "object";
    [SerializeField] protected float AVOID_DISTANCE = 3.0f;
    [SerializeField] protected float AVOID_RADIUS = 0.4f;
    [SerializeField] protected float TURN_SPEED = 10.0f;

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
    }

    protected virtual void FixedUpdate()
    {
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
        Vector3 vel = ComputeVelocity(_currentDir * MOVE_SPEED);
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
        if (x == 0.0f) return;

        float dir = Mathf.Sign(x);

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