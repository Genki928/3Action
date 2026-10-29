using UnityEngine;

public abstract class InteractAssetImpact : InteractAsset
{
    [SerializeField] protected int value = 1;
}

[CreateAssetMenu(menuName = "Item/InteractAsset/Damage")]
public class InteractAssetDamage : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.player.Health.Damage(value);
    }
}

[CreateAssetMenu(menuName = "Item/InteractAsset/Heal")]
public class InteractAssetHeal : InteractAssetImpact
{
    public override void Interact(InteractContext ctx)
    {
        ctx.player.Health.Heal(value);
    }
}