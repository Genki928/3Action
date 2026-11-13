using UnityEngine;

[CreateAssetMenu(menuName = "Item/InteractAsset/Heal")]
public class InteractAssetHeal : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        Debug.Log("umaaaaaaaai2");
        ctx.player.Health.Heal(value);
    }
}