using UnityEngine;

[CreateAssetMenu(menuName = "Item/InteractAsset/PowerEffect")]
public class InteractAssetPowerEffect : InteractAssetEffect
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.Power.AddEffect(new(_value, _time));
    }
}