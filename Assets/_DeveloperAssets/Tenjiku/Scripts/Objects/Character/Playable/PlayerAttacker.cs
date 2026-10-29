using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] GameObject _defaultAttackCollision;

    [SerializeField] float _attackDistance = 1.0f;

    public void Attack(Vector3 vec)
    {
        if (vec == Vector3.zero) return;

        GameObject sword = Instantiate(
            _defaultAttackCollision,
            transform.position,
            Quaternion.identity
        );

        PlayerController player = GetComponent<PlayerController>();

        sword.GetComponent<Sword>().Init(player, vec);
    }
}