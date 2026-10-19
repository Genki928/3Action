using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("▼ 出す敵")]
    [SerializeField] List<GameObject> _enemyPrefabs = new();   // 敵のPrefabのリスト

    [Header("▼ 参照")]
    [SerializeField] Transform _player;
    [SerializeField] Camera _camera;

    [Header("▼ 出現ルール")]
    [SerializeField] float _interval = 3f;       // 何秒ごとに出すか
    [SerializeField] int _maxEnemies = 10;       // 同時に存在できる上限
    [SerializeField] float _minRadius = 12f;     // プレイヤーからの最小距離
    [SerializeField] float _maxRadius = 16f;     // プレイヤーからの最大距離
    [SerializeField] float _viewMargin = 0.1f;   // 画面の外側にどれだけ余裕を取るか

    readonly List<GameObject> _alive = new();
    float _timer;

    void Awake()
    {
        if (_camera == null) _camera = Camera.main;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _interval) return;
        _timer = 0f;

        // 倒された敵(Destroy済み)をリストから外す
        _alive.RemoveAll(e => e == null);
        if (_alive.Count >= _maxEnemies) return;

        Spawn();
    }

    void Spawn()
    {
        if (_enemyPrefabs.Count == 0 || _player == null) return;
        if (!TryGetSpawnPoint(out var pos)) return;

        var prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        var enemy = Instantiate(prefab, pos, Quaternion.identity);
        _alive.Add(enemy);
    }

    // プレイヤーの周りの輪の上から、画面に映っていない場所を探す
    bool TryGetSpawnPoint(out Vector3 pos)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float r = Random.Range(_minRadius, _maxRadius);
            pos = _player.position + new Vector3(dir.x, 0f, dir.y) * r;

            if (IsOffScreen(pos)) return true;
        }
        pos = default;
        return false;
    }

    bool IsOffScreen(Vector3 worldPos)
    {
        var v = _camera.WorldToViewportPoint(worldPos);
        float m = _viewMargin;
        return v.z < 0f || v.x < -m || v.x > 1f + m || v.y < -m || v.y > 1f + m;
    }
}