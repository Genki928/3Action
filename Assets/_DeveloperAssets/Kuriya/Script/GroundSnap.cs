using UnityEngine;

// 生成されたときに、地面の上に底面がぴったり乗るよう高さを合わせる。
// ブッシュなどのプレハブに付けるだけで使える。
public class GroundSnap : MonoBehaviour
{
    [SerializeField] LayerMask GROUND_MASK = ~0;     // 地面として扱うレイヤー(地面だけに絞るのがおすすめ)
    [SerializeField] float RAY_START_HEIGHT = 50.0f; // 真上のどこからレイを飛ばすか
    [SerializeField] float RAY_LENGTH = 200.0f;      // 下へ飛ばす距離
    [SerializeField] float OFFSET_Y = 0.0f;          // 微調整(正で少し浮かせる、負でさらに埋める)

    void Start()
    {
        Snap();
    }

    public void Snap()
    {
        Vector3 origin = transform.position + Vector3.up * RAY_START_HEIGHT;

        RaycastHit[] hits = Physics.RaycastAll(
            origin, Vector3.down, RAY_LENGTH, GROUND_MASK, QueryTriggerInteraction.Ignore);

        // 自分自身のコライダーは無視して、一番近い地面を探す
        float nearest = float.MaxValue;
        float groundY = 0.0f;
        bool found = false;

        foreach (var hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform)) continue;
            if (hit.distance < nearest)
            {
                nearest = hit.distance;
                groundY = hit.point.y;
                found = true;
            }
        }

        if (!found) return;

        // 見た目の底面から、原点までの距離を求める
        Renderer rend = GetComponentInChildren<Renderer>();
        float bottomY = rend != null ? rend.bounds.min.y : transform.position.y;
        float pivotToBottom = transform.position.y - bottomY;

        Vector3 pos = transform.position;
        pos.y = groundY + pivotToBottom + OFFSET_Y;
        transform.position = pos;
    }
}