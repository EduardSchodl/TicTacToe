using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMath
{
    public static bool VecEqualNoZ(this Vector3 vec1, Vector3 vec, float epsilon = 0.001f)
    {
        Vector3 diff = vec - vec1;
        return Math.Abs(diff.x) <= epsilon && Math.Abs(diff.y) <= epsilon;
    }

}
