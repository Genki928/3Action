public class Attribute<T>
{
    // --- プロパティ ---
    public T Value => _currentValue;

    // --- フィールド --- //
    protected T _currentValue;
    protected T _minValue;
    protected T _maxValue;
}