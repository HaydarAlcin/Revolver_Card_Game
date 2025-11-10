public static class FloatExtensions
{
    public static float Normalize360(this float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }
}