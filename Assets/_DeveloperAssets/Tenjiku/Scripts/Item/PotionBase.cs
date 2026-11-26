using UnityEngine;

public class PotionItem : ItemStack
{
    public float ThrowRange
    {
        get
        {
            if (!_ability.Exists(x => x is AbilityAssetThrowRange)) return 0.0f;

            float total = 0.0f;
            foreach (var ability in _ability)
                if (ability is AbilityAssetThrowRange range)
                    total += range.ThrowRange;
            return total;
        }
    }
    public override Sprite Sprite
    {
        get
        {
            foreach (var ability in _ability)
                if (ability is AbilityAssetThrowRange range) return range.Sprite;
            return _sprite;
        }
    }

    public PotionItem(Item item, int count) : base(item, count)
    {
        _item = item;
        _count = count;
    }
}