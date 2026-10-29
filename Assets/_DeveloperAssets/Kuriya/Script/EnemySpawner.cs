
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("▼ 出す敵")]
    [SerializeField] List<GameObject> _enemyPrefabs = new();

    [Header("▼ 参照")]
    [SerializeField] Transform _player;
    [SerializeField] Camera _camera;
    [SerializeField] LegStateManager state;

    [Header("▼ 出現ルール")]
    [SerializeField] float _interval = 3f;
    [SerializeField] int _maxEnemies = 10;
    [SerializeField] float _minRadius = 12f;
    [SerializeField] float _maxRadius = 16f;
    [SerializeField] float _viewMargin = 0.1f;

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

        if (_player == null || _enemyPrefabs.Count == 0)
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
        if (!TryGetSpawnPoint(out Vector3 pos))
        {
            Debug.LogWarning("出現位置が見つかりません。次のフレームで再試行します。");
            return false;
        }

        GameObject prefab =
            _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];

        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

        _alive.Add(enemy);

        Debug.Log($"敵出現成功: {enemy.name} / 生存数: {_alive.Count}");

        return true;
    }



    bool TryGetSpawnPoint(out Vector3 pos)
    {
        // まず画面外の位置を探す
        for (int i = 0; i < 24; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;

            if (dir.sqrMagnitude < 0.001f)
                continue;

            float radius = Random.Range(_minRadius, _maxRadius);

            Vector3 candidate = _player.position
                + new Vector3(dir.x, 0f, dir.y) * radius;

            if (IsOffScreen(candidate))
            {
                pos = candidate;
                return true;
            }
        }

        // 画面外が見つからなければ、距離を広げて再探索
        for (int i = 0; i < 16; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;

            if (dir.sqrMagnitude < 0.001f)
                continue;

            float radius = Random.Range(_maxRadius, _maxRadius * 2f);

            Vector3 candidate = _player.position
                + new Vector3(dir.x, 0f, dir.y) * radius;

            if (IsOffScreen(candidate))
            {
                pos = candidate;
                return true;
            }
        }

        // 最終手段：プレイヤーから離れた位置に出現させる
        Vector2 fallbackDir = Random.insideUnitCircle.normalized;

        if (fallbackDir.sqrMagnitude < 0.001f)
            fallbackDir = Vector2.up;

        pos = _player.position
            + new Vector3(fallbackDir.x, 0f, fallbackDir.y)
            * _maxRadius;

        return true;
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
