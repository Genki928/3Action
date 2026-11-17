using System.Collections.Generic;
using UnityEngine;

public class ValuableAssets<T> : InteractAssetImpact
{
    List<T> value;

    public override void Interact(InteractContext ctx)
    {
        ctx.character.Health.Damage(_value);
    }
}