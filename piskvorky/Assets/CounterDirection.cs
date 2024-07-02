using System.Collections.Generic;
using UnityEngine;

public enum CounterDirection
{
    LEFT,
    RIGHT
}

public static class CounterDirectionExtension
{

    private static Dictionary<CounterDirection, Vector3> DirectionToVector = new Dictionary<CounterDirection, Vector3>();

    static CounterDirectionExtension()
    {
        DirectionToVector.Add(CounterDirection.LEFT, new Vector3(-1, 0));
        DirectionToVector.Add(CounterDirection.RIGHT, new Vector3(1, 0));
    }

    public static Vector3 GetDirectionVector(this CounterDirection direction)
    {
        if(!DirectionToVector.ContainsKey(direction))
        {
            Debug.LogError($"{nameof(DirectionToVector)} doesn't contain an entry for the {direction} direction.");
            return Vector3.zero;
        }

        return DirectionToVector[direction];
    }
}