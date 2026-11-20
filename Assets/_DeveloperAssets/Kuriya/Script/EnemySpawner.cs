using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject prefab;
    [Min(0f)] public float weight = 1f;   // 大きいほど出やすい
}

public class EnemySpawner : MonoBehaviour
{
    [Header("▼ 出す敵")]
    [SerializeField] List<EnemySpawnEntry> _enemies = new();

    [Header("▼ 参照")]
    [SerializeField] Transform _player;
    [SerializeField] Camera _camera;
    [SerializeField] LegStateManager state;

    [Header("▼ 出現ルール")]
    [SerializeField] float _interval = 3f;
    [SerializeField] int _maxEnemies = 10;
    [SerializeField] float _minRadius = 12f;      // これより近くには出さない
    [SerializeField] float _maxRadius = 30f;      // これより遠くには出さない
    [SerializeField] float _radiusStep = 1f;      // 外側へ探す刻み幅
    [SerializeField] float _viewMargin = 0.1f;
    [SerializeField] float _minSpawnDistance = 10f;   // 新規フィールド(Inspectorの旧値に影響されない)
    [SerializeField] float _searchLimit = 60f;        // 画面外を探す最大距離
    [SerializeField] float _searchStep = 1f;
    [SerializeField] float _extraOffset = 2f;         // 画面外になってから、さらに外へずらす距離

    [Header("▼ 安定化")]
    [SerializeField] float _retryInterval = 0.25f;
    [SerializeField] int _candidateCount = 24;

    readonly List<GameObject> _alive = new();

    float _spawnTimer;
    float _retryTimer;

    void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    void Update()
    {
        if (state == null || state.State is not RaidState)
            return;

        if (_player == null || _enemies.Count == 0)
            return;

        _alive.RemoveAll(e => e == null);

        if (_alive.Count >= _maxEnemies)
            return;

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer < _interval)
            return;

        // 出現に成功したときだけタイマーをリセット
        if (TrySpawn())
        {
            _spawnTimer = 0f;
        }
    }

    bool TrySpawn()
    {
        GameObject prefab = PickPrefab();
        if (prefab == null)
        {
            Debug.LogWarning("出現できる敵がありません(重みが全て0 or 未設定)");
            return false;
        }

        if (!TryGetSpawnPoint(out Vector3 pos))
        {
            Debug.LogWarning("出現位置が見つかりません。次のフレームで再試行します。");
            return false;
        }

        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

        _alive.Add(enemy);

        Debug.Log($"敵出現成功: {enemy.name} / 生存数: {_alive.Count}");

        return true;
    }

    /// <summary>重み付きランダムで出現させるプレハブを選ぶ</summary>
    GameObject PickPrefab()
    {
        float total = 0f;
        foreach (var e in _enemies)
        {
            if (e.prefab != null && e.weight > 0f)
                total += e.weight;
        }

        if (total <= 0f)
            return null;

        float r = Random.value * total;

        foreach (var e in _enemies)
        {
            if (e.prefab == null || e.weight <= 0f)
                continue;

            r -= e.weight;
            if (r <= 0f)
                return e.prefab;
        }

        return null; // 浮動小数点誤差の保険
    }

    bool TryGetSpawnPoint(out Vector3 pos)
    {
        for (int i = 0; i < _candidateCount; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            if (dir.sqrMagnitude < 0.001f)
                continue;

            Vector3 dir3 = new Vector3(dir.x, 0f, dir.y);

            for (float r = _minSpawnDistance; r <= _searchLimit; r += _searchStep)
            {
                Vector3 candidate = _player.position + dir3 * r;

                if (IsOffScreen(candidate))
                {
                    // 画面ぎりぎりでポップインしないよう少し外へ
                    pos = _player.position + dir3 * (r + _extraOffset);
                    Debug.Log($"出現方向:{dir} 距離:{r + _extraOffset:F1}");
                    return true;
                }
            }
        }

        pos = default;
        return false;
    }

    bool IsOffScreen(Vector3 worldPos)
    {
        Vector3 v = _camera.WorldToViewportPoint(worldPos);

        float m = _viewMargin;

        return v.z > 0f &&
               (v.x < -m || v.x > 1f + m ||
                v.y < -m || v.y > 1f + m);
    }
}