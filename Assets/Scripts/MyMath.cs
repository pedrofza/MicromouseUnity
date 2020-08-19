using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath 
{
    private static readonly int[] maximumValueRepresentedByQuantityOfBitsCache = {0, 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047, 4095, 8191, 16383, 32767, 65535, 131071, 262143, 524287, 1048575, 2097151, 4194303, 8388607, 16777215, 33554431, 67108863, 134217727, 268435455, 536870911, 1073741823, 2147483647};
    public static int MaximumValueRepresentedByQuantityOfBits(int numberOfBits)
    {
        return maximumValueRepresentedByQuantityOfBitsCache[numberOfBits];
    }
}
