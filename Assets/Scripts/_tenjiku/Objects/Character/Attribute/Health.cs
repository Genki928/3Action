public class Health : Attribute<int>
{
    public Health(int start, int max = 0)
    {
        currentValue = start;
        if (max > 0) maxValue = max;
    }
}