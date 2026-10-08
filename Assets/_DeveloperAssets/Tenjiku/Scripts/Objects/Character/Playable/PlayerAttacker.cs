using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] GameObject _defaultAttackCollision;
    
    public void Attack(Vector3 vec)
    {
        GameObject damage = Instantiate(_defaultAttackCollision, transform.position, Quaternion.identity);
        damage.GetComponent<SimpleDamageCollision>().Init(vec);
    }
}