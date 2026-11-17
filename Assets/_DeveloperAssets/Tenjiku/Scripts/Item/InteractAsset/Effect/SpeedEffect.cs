using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/InteractAsset/SpeedEffect")]
public class InteractAssetSpeedEffect : InteractAssetEffect
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.MoveSpeed.AddEffect(new(_value, _time));
    }
}