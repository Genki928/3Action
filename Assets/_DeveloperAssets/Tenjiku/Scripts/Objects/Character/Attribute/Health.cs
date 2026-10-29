using System;

public class Health : Attribute<int>
{
    // --- イベント ---
    public event Action OnDamage;
    public event Action OnHeal;

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

    // --- メソッド ---
    public void Heal(int heal)
    {
        _currentValue = Math.Min(_currentValue + heal, _maxValue);
        OnHeal?.Invoke();
    }
}