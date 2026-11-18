using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/InteractAsset/Heal")]
public class InteractAssetHeal : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        Debug.Log("umaaaaaaaai2");
        ctx.character.Health.Heal(_value);
    }
}