using UnityEngine;

public abstract class ItemAbility : ScriptableObject
{
    public abstract void Interact(InteractContext ctx);
}

public class InteractContext
{
    public CharacterBase character;

    public InteractContext(CharacterBase character)
    {
        this.character = character;
    }
}