using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScatterEntry
{
    public string name = "Tree";                   // 管理用の名前(Inspectorで見分けるため)
    public GameObject prefab;                      // 生成するプレハブ
    public int count = 30;                         // 生成する数
    public float minSpacing = 2.0f;                // 他のオブジェクトとの最低距離
    public Vector2 scaleRange = new(1.0f, 1.0f);   // ランダムな大きさ(最小, 最大)
    public float yOffset = 0.0f;                   // 高さの微調整(負で少し埋める)
    public bool randomYRotation = false;           // ONなら縦軸まわりにランダムに回す(板に絵を貼った見た目はOFFにする)
}

// シーンを読み込んだときに、範囲内へ木やブッシュを地面に沿って自動配置する。
public class ObjectScatter : MonoBehaviour
{
    [Header("▼ Area")]
    [SerializeField] Vector2 AREA_SIZE = new(100.0f, 100.0f); // 配置する範囲(このオブジェクトを中心にXZ平面)
    [SerializeField] LayerMask GROUND_MASK = ~0;              // 地面のレイヤー(地面だけに絞ること)
    [SerializeField] float RAY_START_HEIGHT = 50.0f;          // 範囲の中心から、どれだけ上からレイを飛ばすか
    [SerializeField] float RAY_LENGTH = 200.0f;
    [SerializeField] float MAX_SLOPE_ANGLE = 30.0f;           // これより急な斜面には置かない

    [Header("▼ Keep Clear(プレイヤーの初期位置などを空けておく)")]
    [SerializeField] Transform KEEP_CLEAR_CENTER;             // 空けておく中心(未設定なら無効)
    [SerializeField] float KEEP_CLEAR_RADIUS = 5.0f;

    [Header("▼ Random")]
    [SerializeField] bool USE_FIXED_SEED = false;             // ONなら毎回同じ配置になる
    [SerializeField] int SEED = 0;
    [SerializeField] int MAX_ATTEMPTS_PER_OBJECT = 30;        // 置ける場所を探す試行回数

    [Header("▼ Objects")]
    [SerializeField] ScatterEntry[] ENTRIES;

    // 他のスクリプト(StageBoundaryなど)から配置範囲を読むため
    public Vector2 AreaSize => AREA_SIZE;

    Transform _container;
    readonly List<(Vector3 pos, float spacing)> _placed = new();

    void Start()
    {
        Scatter();
    }

    public void Scatter()
    {
        if (USE_FIXED_SEED) Random.InitState(SEED);

        // 生成物はまとめて1つの親の下に置く(ヒエラルキーが散らからない)
        _container = new GameObject("Scattered Objects").transform;
        _container.SetParent(transform, false);
        _placed.Clear();

        Physics.SyncTransforms();

        foreach (var entry in ENTRIES)
        {
            if (entry.prefab == null) continue;

            int success = 0;
            for (int i = 0; i < entry.count; i++)
            {
                if (TryPlace(entry)) success++;
            }

            if (success < entry.count)
            {
                Debug.LogWarning($"{entry.name}: {entry.count}個中 {success}個だけ配置できました(範囲が狭い/間隔が広い/斜面が急など)");
            }
        }
    }

    bool TryPlace(ScatterEntry entry)
    {
        for (int attempt = 0; attempt < MAX_ATTEMPTS_PER_OBJECT; attempt++)
        {
            // 範囲内のランダムな位置
            float x = Random.Range(-AREA_SIZE.x * 0.5f, AREA_SIZE.x * 0.5f);
            float z = Random.Range(-AREA_SIZE.y * 0.5f, AREA_SIZE.y * 0.5f);
            Vector3 origin = transform.position + new Vector3(x, RAY_START_HEIGHT, z);

            if (!FindGround(origin, out Vector3 groundPoint, out Vector3 groundNormal)) continue;

            // 急な斜面は避ける
            if (Vector3.Angle(groundNormal, Vector3.up) > MAX_SLOPE_ANGLE) continue;

            // 空けておく範囲
            if (KEEP_CLEAR_CENTER != null)
            {
                Vector3 d = groundPoint - KEEP_CLEAR_CENTER.position;
                d.y = 0.0f;
                if (d.magnitude < KEEP_CLEAR_RADIUS) continue;
            }

            // 他のオブジェクトと近すぎないか
            if (IsTooClose(groundPoint, entry.minSpacing)) continue;

            Spawn(entry, groundPoint);
            _placed.Add((groundPoint, entry.minSpacing));
            return true;
        }
        return false;
    }

    // 上から下へレイを飛ばして、一番近い地面を探す(生成済みの木やブッシュは無視)
    bool FindGround(Vector3 origin, out Vector3 point, out Vector3 normal)
    {
        point = default;
        normal = Vector3.up;

        RaycastHit[] hits = Physics.RaycastAll(
            origin, Vector3.down, RAY_LENGTH, GROUND_MASK, QueryTriggerInteraction.Ignore);

        float nearest = float.MaxValue;
        bool found = false;

        foreach (var hit in hits)
        {
            if (hit.collider.transform.IsChildOf(_container)) continue;
            if (hit.distance < nearest)
            {
                nearest = hit.distance;
                point = hit.point;
                normal = hit.normal;
                found = true;
            }
        }
        return found;
    }

    bool IsTooClose(Vector3 pos, float spacing)
    {
        foreach (var p in _placed)
        {
            Vector3 d = pos - p.pos;
            d.y = 0.0f;
            // 相手と自分、広いほうの間隔を採用
            float need = Mathf.Max(spacing, p.spacing);
            if (d.sqrMagnitude < need * need) return true;
        }
        return false;
    }

    void Spawn(ScatterEntry entry, Vector3 groundPoint)
    {
        // 向きはプレハブ自身の向きを基準にする(ランダム回転はONのときだけ)
        Quaternion rotation = entry.prefab.transform.rotation;
        if (entry.randomYRotation)
        {
            rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f) * rotation;
        }

        GameObject obj = Instantiate(entry.prefab, groundPoint, rotation, _container);

        // Inspectorで0のままだと見えなくなるので、0以下は等倍として扱う
        float scale = Random.Range(entry.scaleRange.x, entry.scaleRange.y);
        if (scale <= 0.0f) scale = 1.0f;
        obj.transform.localScale *= scale;

        // 見た目の底面が地面に接するように高さを合わせる(原点が中心でも埋まらない)
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);

            float pivotToBottom = obj.transform.position.y - b.min.y;
            Vector3 pos = obj.transform.position;
            pos.y = groundPoint.y + pivotToBottom + entry.yOffset;
            obj.transform.position = pos;
        }
    }

    // シーンビューで配置範囲を確認できる
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);
        Gizmos.DrawWireCube(transform.position, new Vector3(AREA_SIZE.x, 0.1f, AREA_SIZE.y));

        if (KEEP_CLEAR_CENTER != null)
        {
            Gizmos.color = new Color(0.9f, 0.3f, 0.3f, 0.8f);
            Gizmos.DrawWireSphere(KEEP_CLEAR_CENTER.position, KEEP_CLEAR_RADIUS);
        }
    }
}