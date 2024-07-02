using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StreakStatusExt
{
    public static int GetBlockageCount(this StreakStatus streakStatus)
    {
        return (int)streakStatus;
    }
}
