using UnityEngine;

public class ItemObj : ObjectBase
{
    // --- プロパティ --- //
    public ItemStack Stack => _item;

    // --- フィールド --- //
    [Header("▼ ItemData")]
    [SerializeField] private float amplitude = 0.5f; // 振れ幅（上下に動く距離）
    [SerializeField] private float speed = 2f;     // 浮遊の速さ
    [SerializeField] ItemStack _item;
    private Vector3 startPos;
    bool _onGround = false;
    Rigidbody _rigidbody;

    // --- メソッド --- //
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // 画像を変更
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = _item.Item.Material;
    }

    void FixedUpdate()
    {
        if (!_onGround) return;

        // 初期位置のY座標を基準にサイン波で上下させる
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    void OnTriggerEnter(Collider col)
    {
        if (_onGround) return;

        if (col.CompareTag("Ground"))
        {
            _onGround = true;
            startPos = transform.position;
        }
    }
}
