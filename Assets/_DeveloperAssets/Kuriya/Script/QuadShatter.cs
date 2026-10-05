using UnityEngine;

/// <summary>
/// Quad を格子状のかけらに分割して、Rigidbody で吹き飛ばすスクリプト。
/// Quad にアタッチするだけで動く。かけらは Start で自動生成される。
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class QuadShatter : MonoBehaviour
{
    [Header("分割数")]
    [SerializeField] int columns = 5;   // 横の分割数
    [SerializeField] int rows = 5;      // 縦の分割数

    [Header("吹き飛び方")]
    [SerializeField] float explosionForce = 6f;   // 吹き飛ばす強さ
    [SerializeField] float torqueForce = 4f;      // 回転の強さ
    [SerializeField] float pieceThickness = 0.05f; // かけらの厚み(当たり判定用)

    [Header("タイミング")]
    [SerializeField] bool autoShatter = true;     // true: 再生後に自動で壊れる
    [SerializeField] float shatterDelay = 2f;     // 自動で壊れるまでの秒数
    [SerializeField] float destroyAfter = 3f;     // 壊れてから消えるまでの秒数

    Material material;
    Rigidbody[] pieces;
    bool shattered;

    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        CreatePieces();

        if (autoShatter)
        {
            Invoke(nameof(Shatter), shatterDelay);
        }
    }

    // かけらを生成する(元の Quad は見えなくしておく)
    void CreatePieces()
    {
        GetComponent<MeshRenderer>().enabled = false;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        pieces = new Rigidbody[columns * rows];
        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var piece = new GameObject($"Piece_{x}_{y}");
                piece.transform.SetParent(transform, false);

                // Quad は -0.5〜0.5 の大きさなので、各かけらの中心と大きさを計算する
                float px = (x + 0.5f) / columns - 0.5f;
                float py = (y + 0.5f) / rows - 0.5f;
                piece.transform.localPosition = new Vector3(px, py, 0f);
                piece.transform.localScale = new Vector3(1f / columns, 1f / rows, 1f);

                piece.AddComponent<MeshFilter>().sharedMesh = CreatePieceMesh(x, y);
                piece.AddComponent<MeshRenderer>().sharedMaterial = material;

                // 当たり判定(Quad は厚みがないので薄い Box を使う)
                var box = piece.AddComponent<BoxCollider>();
                box.size = new Vector3(1f, 1f, pieceThickness / piece.transform.lossyScale.z);

                // 壊れるまでは動かないように Kinematic にしておく
                var rb = piece.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                pieces[index++] = rb;
            }
        }
    }

    // 元のテクスチャの一部分(UV)だけを使うメッシュを作る
    Mesh CreatePieceMesh(int x, int y)
    {
        float u0 = (float)x / columns;
        float u1 = (float)(x + 1) / columns;
        float v0 = (float)y / rows;
        float v1 = (float)(y + 1) / rows;

        var mesh = new Mesh();
        mesh.vertices = new[]
        {
            new Vector3(-0.5f, -0.5f, 0f), // 0: 左下
            new Vector3( 0.5f, -0.5f, 0f), // 1: 右下
            new Vector3(-0.5f,  0.5f, 0f), // 2: 左上
            new Vector3( 0.5f,  0.5f, 0f), // 3: 右上
        };
        mesh.uv = new[]
        {
            new Vector2(u0, v0),
            new Vector2(u1, v0),
            new Vector2(u0, v1),
            new Vector2(u1, v1),
        };
        mesh.normals = new[]
        {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back
        };
        mesh.triangles = new[] { 0, 2, 3, 0, 3, 1 };
        return mesh;
    }

    // 壊す。autoShatter を使わない場合は、他のスクリプトからこれを呼ぶ
    [ContextMenu("Shatter")]
    public void Shatter()
    {
        if (shattered || pieces == null) return;
        shattered = true;

        // 爆発の中心は Quad の少し手前側(-Z 側)にして、手前に飛び散るようにする
        Vector3 center = transform.position - transform.forward * 0.3f;

        foreach (var rb in pieces)
        {
            // 親から切り離して、ワールド上で自由に動けるようにする
            rb.transform.SetParent(null, true);
            rb.isKinematic = false;

            rb.AddExplosionForce(explosionForce, center, 10f, 0f, ForceMode.Impulse);
            // Z軸の回転はさせない(回転の力はX軸とY軸だけにする)
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            Vector3 torque = Random.insideUnitSphere * torqueForce;
            torque.z = 0f;
            rb.AddTorque(torque, ForceMode.Impulse);

            Destroy(rb.gameObject, destroyAfter);
        }

        // 空になった元のオブジェクトも消す
        Destroy(gameObject);
    }
}