using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{
    public static string VectorToFieldString(Vector2 vector)
    {
        return $"{vector.x}x{vector.y}";
    }

    public static Vector2Int CounterToVector(int counter)
    {
        return Vector2Int.one * counter;
    }

    public static IEnumerable<Vector2Int> IterateRectanglePositionsTopLeft(int width, int height)
    {
        for (int y = height - 1; y >= 0; --y)
        {
            for (int x = 0; x < width; ++x)
            {
                yield return new Vector2Int(x, y);
            }
        }

        yield break;
    }

    public static Vector2Int[] GetSquarePositionsTopLeft(int sideLength)
    {
        return GetRectanglePositionsTopLeft(sideLength, sideLength);
    }

    public static Vector2Int[] GetRectanglePositionsTopLeft(int width, int height)
    {
        Vector2Int[] positionsArray = new Vector2Int[width * height];

        int i = 0;
        foreach (Vector2Int position in IterateRectanglePositionsTopLeft(width, height))
        {
            positionsArray[i] = position;
            ++i;
        }

        return positionsArray;
    }

    public static (Vector2Int xVec, Vector2Int yVec) BreakVectorCoordinates(this Vector2Int vec)
    {
        return (xVec: new Vector2Int(vec.x, 0), yVec:  new Vector2Int(0, vec.y));
    }

    public static bool AnyAbsCoordinateIsLargerThan(this Vector2Int vec, int value)
    {
        int x = System.Math.Abs(vec.x);
        int y = System.Math.Abs(vec.y);
        return (x > value) || (y > value);
    }
}
