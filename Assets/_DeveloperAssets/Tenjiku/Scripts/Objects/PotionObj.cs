using System.Collections.Generic;
using UnityEngine;

public class PotionObj : MonoBehaviour
{
    [SerializeField] SimpleDamageCollision _damageArea;
    [SerializeField] List<ItemAbility> _ability;

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Ground")) return;

        SimpleDamageCollision obj = Instantiate(_damageArea, transform.position, new(0.0f, 0.0f, 0.0f, 0.0f));
        obj.Init(new(0.0f, 0.0f, 0.0f), _ability);
        Destroy(gameObject);
    }

    public void SetAbility(Vector3 vec, List<ItemAbility> ability)
    {
        _ability = ability;
        var rigidbody = GetComponent<Rigidbody>();
        Debug.Log(vec);
        rigidbody.linearVelocity = vec;
    }
}
