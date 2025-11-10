using System;

public static class IntExtensions
{
    public static string ToK(this int value)
    {
        long abs = Math.Abs((long)value);
        if (abs < 1000) return value.ToString() + "X";

        long k = abs / 1000;
        return (value < 0 ? "-" : "") + k + "K";
    }
}
