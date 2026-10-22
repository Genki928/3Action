using UnityEngine;

public class ItemObj : ObjectBase
{
    public ItemStack Stack => _item;
    [SerializeField] ItemStack _item;
    bool _onGround = false;
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = _item.Item.Material;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _onGround = true;
    }
}
