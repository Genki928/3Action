public class Power : ModifierAttribute<int>
{
    // --- プロパティ --- //
    public override int Value
    {
        get
        {
            // 合計を取得
            int total = 0;
            foreach (Effect ef in effects)
                total += ef.Value;

            // 算出
            return _currentValue + total;
        }
    }

    // --- コンストラクタ --- //
    public Power(int start)
    {
        _currentValue = start;
    }
}