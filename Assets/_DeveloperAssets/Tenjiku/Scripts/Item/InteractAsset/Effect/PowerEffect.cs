using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/InteractAsset/PowerEffect")]
public class InteractAssetPowerEffect : InteractAssetEffect
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.Power.AddEffect(new(_value, _time));
    }
}