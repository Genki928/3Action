using System;

public class Power : Attribute<int>
{
    // --- コンストラクタ ---
    public Power(int start)
    {
        _currentValue = start;
    }
}