using UnityEngine;

[CreateAssetMenu(menuName = "Item/InteractAsset/Damage")]
public class InteractAssetDamage : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.Health.Damage(_value);
    }
}