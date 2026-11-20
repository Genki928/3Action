using UnityEngine;

public class SimpleDamageCollision : DamageCollisionBase
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<CharacterBase>(out var cb))
        {
            InteractContext ictx = new(cb);

            // メインハンドに持つアイテムが持つ、実行時におこる処理を実行
            Debug.Log("korsosu");
            foreach (var ability in _ability)
            {
                // 実行時効果を持つなら関数を実行
                if (ability is InteractAbility impact)
                {
                    impact.Interact(ictx);
                }
            }
        }
        if (col.CompareTag("Player")) return;
    }
}