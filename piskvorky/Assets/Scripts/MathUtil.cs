using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathUtil
{

    public static int GetDigitCount(int number)
    {
        return number.ToString().Length;
    }

    public static IEnumerable<int> IterateDigitsLeftToRight(int number)
    {
        return Enumerable.Reverse(IterateDigitsRightToLeft(number));
    }

    public static IEnumerable<int> IterateDigitsRightToLeft(int number)
    {
        while(number != 0)
        {
            int digit = number % 10;
            yield return digit;
            number /= 10;
        }
    }

    public static int IntPow(int num, int exp)
    {
        Debug.LogWarning("This method hasn't been tested very much. Use at your own risk.");
        if(exp < 0)
        {
            Debug.LogError($"{nameof(IntPow)} doesn't allow negative exponents.");
            return 0;
        }

        if(exp == 0)
        {
            return 1;
        }

        int originalNum = num;
        int result = num;

        for(int currentExponent = 2; currentExponent <= exp; ++currentExponent)
        {
            result *= originalNum;
        }

        return result;
    }

    public static int IntClamp(int value, int minValue, int maxValue)
    {
        if(value < minValue)
        {
            return minValue;
        }
        else if(value > maxValue)
        {
            return maxValue;
        }
        else
        {
            return value;
        }
    }

    public static int GetMaxIndexFromBetweenValues(int min, int max)
    {
        int diff = Math.Abs(max - min);
        return diff;
    }
    
    public static bool DoubleEquals(double d, double d1, double epsilon = 0.001f)
    {
        return Math.Abs(d1 - d) <= epsilon;
    }

    public static bool IsDoubleWhole(double d)
    {
        return DoubleEquals(d % 1, 0);
    }

    public static int IntPow2(int x)
    {
        return x * x;
    }

    public static (bool makesSquare, int squareLength) FieldStrMakesSquare(int fieldLength)
    {
        // 1x1 = 1 open square + 2 separators (2*1) = 3
        // 2x2 = 4 squares + 6 separators (3*2) = 10
        // 3x3 = 9 squares + 12 separators (4*3) = 21
        // 4x4 = 16 squares + 20 separators (5*4) = 36
        // 5*5 = 25 squares + 30 separators (6*5) = 55
        // ...
        // intended square: x
        // x by x
        // y = x^2 + (x * (x + 1)))
        // x^2 + x + x^2

        // x1 is always the larger, we don't want the negative one
        double? x1 = SolveQuadratic(2, 1, -fieldLength).x1;

        if(!x1.HasValue)
        {
            Debug.LogError($"{fieldLength} produces incomputable value from {nameof(SolveQuadratic)}");
            return (false, 0);
        }

        bool makesSquare = IsDoubleWhole(x1.Value);
        
        if(!makesSquare)
        {
            return (false, 0);
        }

        return (true, Convert.ToInt32(x1.Value));
    }



    public static (double? x1, double? x2) SolveQuadratic(double a, double b, double c)
    {
        double d = (b * b) - (4 * a * c);

        if(d < 0)
        {
            return (null, null);
        }

        double x1 = (-b + Math.Sqrt(d)) / (2 * a);

        if(d == 0)
        {
            return (x1, null);
        }

        double x2 = (-b - Math.Sqrt(d)) / (2 * a);

        return (x1, x2);
    }

}
