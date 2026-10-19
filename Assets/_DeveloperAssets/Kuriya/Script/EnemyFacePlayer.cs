using UnityEngine;

public class EnemyFacePlayer : MonoBehaviour
{
    [SerializeField] AnimControl _anim;   // 敵のQuadのAnimControl
    Transform _player;

    void Start()
    {
        // スポナーで出した敵はPlayerを手で指定できないので、タグで探す
        var p = GameObject.FindWithTag("Player");
        if (p != null) _player = p.transform;
    }

    void Update()
    {
        if (_player == null || _anim == null) return;

        bool playerIsRight = _player.position.x > transform.position.x;
        _anim.SetFlip(playerIsRight);
    }
}