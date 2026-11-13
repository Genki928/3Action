using System;

public class MoveSpeed : Attribute<float>
{
    // --- コンストラクタ ---
    public MoveSpeed(float start)
    {
        _currentValue = start;
    }

    public void AddEffect(EffectType effect)
    {

    }
}