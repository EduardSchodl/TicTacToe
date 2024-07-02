using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

public static class DebugUtil
{
    public static void PrintStopwatchTime(Stopwatch stopwatch)
    {
        Debug.Log(string.Format("{0:00}.{1:00}", stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds));
    }
}
