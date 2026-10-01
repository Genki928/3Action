using UnityEngine;

public class CharacterBase : ObjectBase
{
    protected Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
}