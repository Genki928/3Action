using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    // --- プロパティ ---
    public Inventory Inventory => _inventory;

    // --- フィールド ---
    [Header("▼ Camera")]
    [SerializeField] Transform _camera;
    [SerializeField] float DISTANCE_Y, DISTANCE_Z;
    Vector3 _moveVec;

    [Header("▼ Phisics")]
    [SerializeField] float MOVE_SPEED = 5.0f;
    [SerializeField] int START_HEALTH = 4;
    Vector3 _direction;

    [Header("▼ Interact")]
    [SerializeField] float INTERACT_RANGE = 2.0f; // 木を壊せる距離
    [SerializeField] PlayerAttacker _attacker;

    [Header("▼ Animation")]
    [SerializeField] AnimControl _anim;

    [Header("▼ Inventory")]
    Inventory _inventory = new();
    [SerializeField]InventoryManager inventory;
    // --- メソッド ---
    protected override void Start()
    {
        base.Start();

        // ステータス設定
        _moveSpeed = new(MOVE_SPEED);
        _health = new(START_HEALTH);
    }

    void LateUpdate()
    {
        _camera.position = new(transform.position.x, transform.position.y + DISTANCE_Y, transform.position.z + DISTANCE_Z);
    }

    void FixedUpdate()
    {
        if (_isKnockBack)
            return;

        //ベクトルの補正
        var vec = _isPaused ? 0.0f : _moveSpeed.Value;
        _rigidbody.linearVelocity = _moveVec * vec;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        // 移動ベクトルの保存
        var vec = ctx.ReadValue<Vector2>();
        _moveVec = new(vec.x, 0.0f, vec.y);
        if (!(vec.x == 0.0f && vec.y == 0.0f)) _direction = _moveVec;

        // 動いていれば歩行、止まっていればアイドル
        _anim.Play(vec == Vector2.zero ? "Idle" : "Walk");

        // 横入力がないときは、直前の向きを保つ
        if (vec.x == 0.0f) return;

        // 右入力のとき反転(元のコードと同じ向き)
        _anim.SetFlip(vec.x > 0.0f);
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        // ボタンを押した瞬間の1回だけ処理する
        if (!ctx.performed) return;
        if (_isPaused) return;

        //持っているアイテムが武器なら
        if (inventory.Cursor.SelectingItem.Stack.Item.Name=="ベリー、アリ")
        {
            Attack();
        }

        // 自分の周り(INTERACT_RANGE の球)にあるものを調べる
        var hits = Physics.OverlapSphere(transform.position, INTERACT_RANGE);
        foreach (var hit in hits)
        {
            // 木(QuadShatter が付いたオブジェクト)なら壊す
            var tree = hit.GetComponentInParent<QuadShatter>();
            if (tree != null)
            {
                tree.Shatter(transform.position); // プレイヤーから離れる向きに飛び散る
            }
            var bush = hit.GetComponentInParent<Bush>();
            if(bush != null)
            {
                bush.Interact(_inventory);
            }
            
        }
    }

    void Attack()
    {
        Debug.Log("攻撃");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<ItemObj>(out var item))
        {
            Inventory.AddItem(item.Stack);
            Destroy(item.gameObject);
        }
    }
}