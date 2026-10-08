using UnityEngine;

public class DamageCollisionBase : ObjectBase
{
    [SerializeField] protected int _damage;
    [SerializeField] protected float _duration;
    [SerializeField] protected float _speed;
    protected Rigidbody _rigidbody;
    protected GameObject _owner;
    protected Vector3 _vector;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (_isPaused) return;

        _rigidbody.linearVelocity = _vector * _speed;

        _duration -= Time.deltaTime;
        if (_duration < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 vector)
    {
        _vector = vector;
    }
}