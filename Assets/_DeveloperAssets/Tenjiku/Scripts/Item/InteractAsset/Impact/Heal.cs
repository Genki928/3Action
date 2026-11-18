using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/InteractAsset/Heal")]
public class InteractAssetHeal : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.Health.Heal(_value);
    }
}