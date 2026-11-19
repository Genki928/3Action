using System.Collections.Generic;
using UnityEngine;

// ステージの端に「透明な壁」を作り、その境界線の地面から赤い光のカーテンを立ち上げる。
// シーン開始時に自動で生成する。
public class StageBoundary : MonoBehaviour
{
    [Header("▼ Area")]
    [SerializeField] ObjectScatter MATCH_SCATTER;           // 設定すると、その配置範囲に自動で合わせる
    [SerializeField] Vector2 AREA_SIZE = new(100.0f, 100.0f); // MATCH_SCATTER未設定のときの範囲(このオブジェクトが中心)
    [SerializeField] float INSET = 0.0f;                    // 範囲を内側へ縮める量(木が境界を越えないようにしたいとき)
    [SerializeField] LayerMask GROUND_MASK = ~0;            // 地面のレイヤー(光を地面に沿わせるのに使う)
    [SerializeField] float RAY_START_HEIGHT = 50.0f;
    [SerializeField] float RAY_LENGTH = 200.0f;

    [Header("▼ Wall(見えない壁)")]
    [SerializeField] float WALL_HEIGHT = 30.0f;             // 飛ぶ敵(Ghost/Bat)も止めるため高くする
    [SerializeField] float WALL_THICKNESS = 2.0f;

    [Header("▼ Red Light(赤い光)")]
    [SerializeField] Color LIGHT_COLOR = new(1.0f, 0.1f, 0.1f, 1.0f);
    [SerializeField] float LIGHT_HEIGHT = 4.0f;             // 光の高さ
    [SerializeField] float SEGMENT_LENGTH = 2.0f;           // 地面の凹凸に沿わせる細かさ(小さいほど細かい)
    [SerializeField] float PULSE_SPEED = 2.0f;              // 明滅の速さ
    [SerializeField, Range(0.0f, 1.0f)] float PULSE_AMOUNT = 0.3f; // 明滅の強さ
    [SerializeField] Material LIGHT_MATERIAL;               // 未設定なら Sprites/Default で自動作成

    Material _lightMaterial;
    Vector2 _half; // 壁の内側の半分の大きさ

    void Start()
    {
        Vector2 size = MATCH_SCATTER != null ? MATCH_SCATTER.AreaSize : AREA_SIZE;
        _half = new Vector2(
            Mathf.Max(1.0f, size.x * 0.5f - INSET),
            Mathf.Max(1.0f, size.y * 0.5f - INSET));

        Physics.SyncTransforms();

        CreateWalls();
        CreateLight();
    }

    // ---- 壁 ----
    void CreateWalls()
    {
        Vector3 c = transform.position;
        float t = WALL_THICKNESS;

        // 下は地面より10m深くまで、上は WALL_HEIGHT まで
        float sizeY = WALL_HEIGHT + 10.0f;
        float centerY = c.y + (WALL_HEIGHT - 10.0f) * 0.5f;

        // 東西(X方向の端)
        MakeWall("Wall East", new Vector3(c.x + _half.x + t * 0.5f, centerY, c.z), new Vector3(t, sizeY, _half.y * 2.0f + t * 2.0f));
        MakeWall("Wall West", new Vector3(c.x - _half.x - t * 0.5f, centerY, c.z), new Vector3(t, sizeY, _half.y * 2.0f + t * 2.0f));
        // 南北(Z方向の端)
        MakeWall("Wall North", new Vector3(c.x, centerY, c.z + _half.y + t * 0.5f), new Vector3(_half.x * 2.0f + t * 2.0f, sizeY, t));
        MakeWall("Wall South", new Vector3(c.x, centerY, c.z - _half.y - t * 0.5f), new Vector3(_half.x * 2.0f + t * 2.0f, sizeY, t));
    }

    void MakeWall(string wallName, Vector3 worldCenter, Vector3 size)
    {
        var go = new GameObject(wallName);
        go.transform.SetParent(transform, false);
        go.transform.position = worldCenter;

        var box = go.AddComponent<BoxCollider>();
        box.size = size;
    }

    // ---- 赤い光 ----
    void CreateLight()
    {
        Vector3 c = transform.position;

        // 境界線の4隅(反時計回りで1周して、最初の点に戻る)
        Vector3[] corners =
        {
            new(c.x - _half.x, 0, c.z - _half.y),
            new(c.x + _half.x, 0, c.z - _half.y),
            new(c.x + _half.x, 0, c.z + _half.y),
            new(c.x - _half.x, 0, c.z + _half.y),
            new(c.x - _half.x, 0, c.z - _half.y),
        };

        // 境界線上に細かく点を並べる
        var points = new List<Vector3>();
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 a = corners[i];
            Vector3 b = corners[i + 1];
            int steps = Mathf.Max(1, Mathf.CeilToInt(Vector3.Distance(a, b) / Mathf.Max(0.1f, SEGMENT_LENGTH)));
            for (int s = 0; s < steps; s++)
            {
                points.Add(Vector3.Lerp(a, b, (float)s / steps));
            }
        }
        points.Add(corners[0]); // 1周して閉じる

        // 下端は赤くて不透明、上端は透明のグラデーション(頂点カラー)
        Color bottomColor = LIGHT_COLOR;
        Color topColor = new Color(LIGHT_COLOR.r, LIGHT_COLOR.g, LIGHT_COLOR.b, 0.0f);

        var vertices = new List<Vector3>();
        var colors = new List<Color>();
        var triangles = new List<int>();

        for (int i = 0; i < points.Count; i++)
        {
            float groundY = GetGroundY(points[i]) + 0.05f; // 地面にめり込まないよう少し浮かせる

            vertices.Add(new Vector3(points[i].x, groundY, points[i].z));
            colors.Add(bottomColor);
            vertices.Add(new Vector3(points[i].x, groundY + LIGHT_HEIGHT, points[i].z));
            colors.Add(topColor);

            if (i == 0) continue;

            int b0 = (i - 1) * 2; // 前の点の下
            int t0 = b0 + 1;      // 前の点の上
            int b1 = i * 2;       // 今の点の下
            int t1 = b1 + 1;      // 今の点の上

            triangles.Add(b0); triangles.Add(t0); triangles.Add(b1);
            triangles.Add(t0); triangles.Add(t1); triangles.Add(b1);
        }

        var mesh = new Mesh { name = "Boundary Light" };
        mesh.SetVertices(vertices);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();

        var go = new GameObject("Boundary Light");
        go.transform.SetParent(transform, false);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;

        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        // マテリアル(両面表示・透明に対応した Sprites/Default を既定で使う)
        if (LIGHT_MATERIAL != null)
        {
            _lightMaterial = new Material(LIGHT_MATERIAL);
        }
        else
        {
            _lightMaterial = new Material(Shader.Find("Sprites/Default"));
        }
        renderer.material = _lightMaterial;
    }

    // 指定した位置の真上から下へレイを飛ばして、地面の高さを求める
    float GetGroundY(Vector3 p)
    {
        Vector3 origin = new Vector3(p.x, transform.position.y + RAY_START_HEIGHT, p.z);

        RaycastHit[] hits = Physics.RaycastAll(
            origin, Vector3.down, RAY_LENGTH, GROUND_MASK, QueryTriggerInteraction.Ignore);

        float nearest = float.MaxValue;
        float y = transform.position.y;

        foreach (var hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform)) continue; // 自分の壁は無視
            if (hit.distance < nearest)
            {
                nearest = hit.distance;
                y = hit.point.y;
            }
        }
        return y;
    }

    // 赤い光をゆっくり明滅させる
    void Update()
    {
        if (_lightMaterial == null || !_lightMaterial.HasProperty("_Color")) return;

        float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * PULSE_SPEED);
        float a = 1.0f - PULSE_AMOUNT * pulse;
        _lightMaterial.color = new Color(1.0f, 1.0f, 1.0f, a);
    }

    // ---- 他のスクリプトから使う ----

    // ステージの内側に収めた位置を返す(敵の出現位置などに使う)。margin は端から空ける距離。
    public Vector3 ClampToStage(Vector3 pos, float margin = 0.5f)
    {
        Vector3 c = transform.position;
        pos.x = Mathf.Clamp(pos.x, c.x - _half.x + margin, c.x + _half.x - margin);
        pos.z = Mathf.Clamp(pos.z, c.z - _half.y + margin, c.z + _half.y - margin);
        return pos;
    }

    // シーンビューで境界線を確認できる
    void OnDrawGizmosSelected()
    {
        Vector2 size = MATCH_SCATTER != null ? MATCH_SCATTER.AreaSize : AREA_SIZE;
        Vector2 half = new Vector2(Mathf.Max(1.0f, size.x * 0.5f - INSET), Mathf.Max(1.0f, size.y * 0.5f - INSET));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (WALL_HEIGHT * 0.5f),
            new Vector3(half.x * 2.0f, WALL_HEIGHT, half.y * 2.0f));
    }
}