using UnityEngine;

public class Ghost : EnemyBase
{
    [Header("▼ HuyoHuyo")]
    [SerializeField] float HOVER_HEIGHT = 10.0f;   // 浮く高さの中心
    [SerializeField] float FLOAT_AMPLITUDE = 0.3f; // 上下の揺れ幅
    [SerializeField] float FLOAT_SPEED = 2.0f;     // 揺れる速さ
    [SerializeField] float SWAY_AMOUNT = 0.3f;     // 横の揺らぎの強さ
    [SerializeField] float HEIGHT_FOLLOW = 5.0f;   // 高さへの追従の強さ

    float _floatOffset; // ゴーストごとに揺れをずらす

    protected override void Start()
    {
        base.Start();

        // Rigidbodyの重力を切る(ふわふわ浮かせるため)
        _rigidbody.useGravity = false;

        // 複数体いても揺れがバラけるように、ランダムな位相を持たせる
        _floatOffset = Random.value * Mathf.PI * 2.0f;
    }

    // 目標の高さ(ふよふよ)。ターゲットの有無に関わらず使う
    float GetTargetY()
    {
        return HOVER_HEIGHT
            + Mathf.Sin(Time.time * FLOAT_SPEED + _floatOffset) * FLOAT_AMPLITUDE;
    }

    // ターゲットがいないときは、その場でふよふよだけする
    protected override void OnNoTarget()
    {
        _rigidbody.linearVelocity =
            new Vector3(0, (GetTargetY() - transform.position.y) * HEIGHT_FOLLOW, 0);
    }

    protected override Vector3 ComputeVelocity(Vector3 vel)
    {
        // 横の揺らぎ: 進行方向に対して左右にゆらゆらさせる
        Vector3 side = Vector3.Cross(Vector3.up, _currentDir);
        float sway = Mathf.Sin(Time.time * FLOAT_SPEED * 0.7f + _floatOffset);
        vel += side * sway * SWAY_AMOUNT;

        // 縦方向: 目標の高さに向かって速度を出す
        vel.y = (GetTargetY() - transform.position.y) * HEIGHT_FOLLOW;
        return vel;
    }
}