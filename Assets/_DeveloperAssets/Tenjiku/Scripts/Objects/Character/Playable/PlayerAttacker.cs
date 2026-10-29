using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] GameObject _defaultAttackCollision;

    [SerializeField] float _attackDistance = 1.0f;

    // Œ»İoŒ»‚µ‚Ä‚¢‚éŒ•
    GameObject _currentSword;

    public void Attack(Vector3 vec)
    {
        // Œ•‚ª‚Ü‚¾‘¶İ‚µ‚Ä‚¢‚é‚È‚çUŒ‚‚µ‚È‚¢
        if (_currentSword != null) return;

        if (vec == Vector3.zero) return;

        _currentSword = Instantiate(
            _defaultAttackCollision,
            transform.position,
            Quaternion.identity
        );

        PlayerController player = GetComponent<PlayerController>();

        _currentSword.GetComponent<Sword>().Init(player, vec);
    }
}