using System;

public class Health : Attribute<int>
{
    // --- イベント ---
    public event Action OnDamage;

    // --- コンストラクタ ---
    public Health(int start, int max = 0)
    {
        _currentValue = start;
        if (max > 0) _maxValue = max;
    }

    // --- メソッド ---
    public void Damage(int damage)
    {
        _currentValue = Math.Max(0, _currentValue - damage);
        OnDamage?.Invoke();
    }
}