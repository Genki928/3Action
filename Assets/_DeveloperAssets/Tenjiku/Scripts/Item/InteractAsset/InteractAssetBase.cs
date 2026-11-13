using UnityEngine;

public abstract class InteractAsset : ScriptableObject
{
    public abstract void Interact(InteractContext ctx);
}

public class InteractContext
{
    public PlayerController player;

    public InteractContext(PlayerController player)
    {
        this.player = player;
    }
}