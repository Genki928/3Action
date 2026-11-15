public class MoveSpeed : ModifierAttribute<float>
{
    // --- プロパティ --- //
    public override float Value
    {
        get
        {
            // 合計を取得
            float total = 0.0f;
            foreach (Effect ef in effects)
                total += ef.Value;

            // 算出
            return _currentValue * ((100.0f + total) / 100.0f);
        }
    }

    // --- コンストラクタ --- //
    public MoveSpeed(float start)
    {
        _currentValue = start;
    }
}