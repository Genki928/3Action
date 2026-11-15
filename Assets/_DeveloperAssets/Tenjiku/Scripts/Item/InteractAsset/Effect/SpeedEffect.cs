using UnityEngine;

[CreateAssetMenu(menuName = "Item/InteractAsset/SpeedEffect")]
public class InteractAssetSpeedEffect : InteractAssetEffect
{
    public override void Interact(InteractContext ctx)
    {
        ctx.character.MoveSpeed.AddEffect(new(_value, _time));
    }
}