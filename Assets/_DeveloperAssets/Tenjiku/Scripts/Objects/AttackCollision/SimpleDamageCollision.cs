using UnityEngine;

public class SimpleDamageCollision : DamageCollisionBase
{
    public void OnTriggerEnter(Collider col)
    {
        var obj = col.gameObject;
        if (obj.CompareTag("Player")) return;

        if (obj.TryGetComponent<TestEnemy>(out var cb))
        {
            //cb.Health.Damage(_damage);
            Destroy(cb.gameObject);
            Debug.Log("test");
        }
    }
}