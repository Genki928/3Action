using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/Damage")]
public class AbilityAssetThrowRange : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.Health.Damage(_value);
    }
}