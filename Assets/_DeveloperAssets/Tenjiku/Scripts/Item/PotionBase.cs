public class PotionItem : ItemStack
{
    public float ThrowRange => _range;
    float _range = 0.0f;

    public PotionItem(Item item, int count) : base(item, count)
    {
        _item = item;
        _range = 0.0f;
        _sprite = Item.Sprite;

        // 飛距離を持つなら画像を変更
        foreach (var ability in _ability)
        {
            if (ability is AbilityAssetThrowRange range)
            {
                _range += range.ThrowRange;
                _sprite = range.Sprite;
            }
        }
    }
}