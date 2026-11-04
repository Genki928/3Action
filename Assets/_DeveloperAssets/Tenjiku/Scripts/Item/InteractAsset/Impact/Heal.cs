using UnityEngine;

[CreateAssetMenu(menuName = "Item/InteractAsset/Heal")]
public class InteractAssetHeal : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.player.Health.Heal(value);
    }
}