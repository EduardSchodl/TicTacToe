using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Assertions
{
    public static void AssertNull<T>(T src, string additionalMessage = "")
    {
        if(src == null)
        {
            return;
        }

        throw new System.Exception($"{src} with type: {src.GetType().Name} must be null. {additionalMessage}");
    }

    public static void AssertStatementNotReached(string additionalMessage = "")
    {
        throw new System.Exception($"This statement should not be reached. {additionalMessage}");
    }
}
