using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{


    [SerializeField] GameObject _defaultAttackCollision;

    public void Attack(InputAction.CallbackContext contxt)
    {
        Debug.Log("Attack!");

    }
    public void Attack(Vector3 vec)
    {
        GameObject damage = Instantiate(_defaultAttackCollision, transform.position, Quaternion.identity);
        damage.GetComponent<SimpleDamageCollision>().Init(vec);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("çUåÇ");
        }
    }
}