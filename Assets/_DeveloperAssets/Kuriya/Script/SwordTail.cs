
using UnityEngine;

// 剣の切っ先に青い軌跡を発生させる
public class SwordTrail : MonoBehaviour
{
    [Header("切っ先のローカル座標")]
    [SerializeField]
    Vector3 TIP_LOCAL_POSITION =
        new Vector3(0.5f, 0f, -0.8f);

    [Header("軌跡設定")]
    [SerializeField] float TRAIL_TIME = 0.25f;
    [SerializeField] float START_WIDTH = 0.25f;
    [SerializeField] float MIN_VERTEX_DISTANCE = 0.01f;

    [Header("色")]
    [SerializeField]
    Color START_COLOR =
        new Color(0.3f, 0.8f, 1f, 0.9f);

    [SerializeField]
    Color END_COLOR =
        new Color(0f, 0.3f, 1f, 0f);

    [Header("描画設定")]
    [SerializeField] Material TRAIL_MATERIAL;
    [SerializeField] string SORTING_LAYER = "Default";
    [SerializeField] int SORTING_ORDER = 10;

    GameObject trailObject;
    TrailRenderer trail;

    bool started;

    void Awake()
    {
        // 軌跡オブジェクトを生成
        trailObject = new GameObject("Sword Tip Trail");

        trail = trailObject.AddComponent<TrailRenderer>();

        trail.time = TRAIL_TIME;
        trail.minVertexDistance = MIN_VERTEX_DISTANCE;
        trail.alignment = LineAlignment.View;

        trail.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        trail.receiveShadows = false;

        trail.sortingLayerName = SORTING_LAYER;
        trail.sortingOrder = SORTING_ORDER;

        // 太さ
        trail.widthCurve = new AnimationCurve(
            new Keyframe(0f, START_WIDTH),
            new Keyframe(1f, 0f)
        );

        // 色
        Gradient gradient = new Gradient();

        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(START_COLOR, 0f),
                new GradientColorKey(END_COLOR, 1f)
            },
            new[]
            {
                new GradientAlphaKey(START_COLOR.a, 0f),
                new GradientAlphaKey(END_COLOR.a, 1f)
            }
        );

        trail.colorGradient = gradient;

        // マテリアル
        trail.material = TRAIL_MATERIAL != null
            ? TRAIL_MATERIAL
            : new Material(Shader.Find("Sprites/Default"));

        trail.emitting = false;
    }

    void LateUpdate()
    {
        // 切っ先のワールド座標を取得
        Vector3 tipPosition =
            transform.TransformPoint(TIP_LOCAL_POSITION);

        trailObject.transform.position = tipPosition;

        // 初回位置を確定してから軌跡開始
        if (!started)
        {
            started = true;

            trail.Clear();
            trail.emitting = true;
        }
    }

    // Sceneビューで切っ先の位置を表示
    void OnDrawGizmos()
    {
        Vector3 tipPosition =
            transform.TransformPoint(TIP_LOCAL_POSITION);

        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(tipPosition, 0.08f);

        Gizmos.DrawLine(
            transform.position,
            tipPosition
        );
    }

    void OnDestroy()
    {
        if (trailObject == null) return;

        trail.emitting = false;

        // 剣が消えても軌跡を残す
        trailObject.transform.SetParent(null);

        Destroy(trailObject, TRAIL_TIME + 0.1f);
    }
}
