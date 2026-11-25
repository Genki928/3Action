using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    // --- プロパティ --- //
    public Inventory Inventory => _inventory;
    public int AttackPower => _power.Value;
    // --- フィールド --- //
    [Header("▼ Camera")]
    [SerializeField] Transform _camera;
    [SerializeField] float DISTANCE_Y, DISTANCE_Z;
    Vector3 _moveVec;

    [Header("▼ Physics")]
    [SerializeField] float MOVE_SPEED = 5.0f;
    [SerializeField] int START_HEALTH = 4;
    Vector3 _direction;

    [Header("▼ Interact")]
    [SerializeField] float INTERACT_RANGE = 2.0f; // 木を壊せる距離
    [SerializeField] PlayerAttacker _attacker;

    [Header("▼ Animation")]
    [SerializeField] AnimControl _anim;

    [Header("▼ Inventory")]
    [SerializeField] InventoryManager inventory;
    Inventory _inventory = new();

    [Header("▼ Attack")]
    [SerializeField] int ATTACK_POWER = 1;
    [SerializeField] PotionObj _potion;


    // --- メソッド --- //
    protected override void Start()
    {
        base.Start();

        // ステータス設定
        _moveSpeed = new(MOVE_SPEED);
        _power = new(ATTACK_POWER);
        _health = new(START_HEALTH, 4);
        //_moveSpeed.AddEffect(new(100, 6.0f));
    }

    protected override void Update()
    {
        base.Update();
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
        if (!ctx.performed || inventory.IsOpenUI || _isPaused) return;

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
            if (bush != null)
            {
                bush.Interact(_inventory);
            }
        }

        if (inventory.MainhandItem.Item == null) return;

        //持っているアイテムが武器なら
        if (inventory.MainhandItem.Item.Name == "剣")
        {
            Attack();
        }

        bool check = false;
        var stack = inventory.MainhandItem;

        // メインハンドに持つアイテムが持つ、実行時におこる処理を実行
        InteractContext ictx = new(this);
        if (stack is PotionItem test)
            Debug.Log(test.ThrowRange);
        if (stack is PotionItem potion && potion.ThrowRange > 0.0f)
        {
            // 生成
            Vector3 pos = new(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            var obj = Instantiate(_potion, pos, Quaternion.identity);
            
            // ベクトル、効果の受け渡し
            var range = potion.ThrowRange;
            Vector3 vec = new(_direction.x * range, 3.0f, _direction.z * range);
            obj.SetAbility(vec, stack.Ability);
            
            // 終了
            _inventory.RemoveItem(inventory.MainhandIndexOfSlot, 1);
            Inventory.UpdateResultItems();
            return;
        }

        foreach (var ability in stack.Ability)
        {
            // 実行時効果を持つなら関数を実行
            if (ability is InteractAbility impact)
            {
                impact.Interact(ictx);
                check = true;
            }
        }

        // 使い切りなら減少
        if (check && stack.Item.Once)
            _inventory.RemoveItem(inventory.MainhandIndexOfSlot, 1);
        Inventory.UpdateResultItems();
    }

    void Attack()
    {
        // 攻撃処理をPlayerAttackerに任せる
        Debug.Log("Ataaaaaaaaaaaaaaaack");
        _attacker.Attack(_direction);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.TryGetComponent<ItemObj>(out var item) && item.PickupCooltime == 0.0f)
        {
            Inventory.AddItem(item.Stack);
            Inventory.UpdateResultItems();
            Debug.Log("aaaaaaaaa" + Inventory.ResultItems[1]);
            Destroy(item.gameObject);
        }
    }

    void ItemOver(ItemStack stack)
    {
        ItemGenerator.Instance.ItemGenerate(transform.position, stack, 3.0f);
    }

    void OnEnable()
    {
        _inventory.OnOverItem += ItemOver;
    }

    void OnDisable()
    {
        _inventory.OnOverItem -= ItemOver;
    }
}