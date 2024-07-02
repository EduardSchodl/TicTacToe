using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupSettingValues : MonoBehaviour
{
    [SerializeField]
    private int minSize = 3;
    public int MinSize => minSize;

    [SerializeField]
    private int maxSize = 15;
    public int MaxSize => maxSize;

    [SerializeField]
    private int minWin = 3;
    public int MinWin => minWin;

    [SerializeField]
    private int maxWin = 8;
    public int MaxWin => maxWin;

    [SerializeField]
    private int minMatchCount = 1;
    public int MinMatchCount => minMatchCount;

    [SerializeField]
    private int maxMatchCount = 10;
    public int MaxMatchCount => maxMatchCount;



    public int GetMaxSizeIndex()
    {
        return MathUtil.GetMaxIndexFromBetweenValues(minSize, maxSize);
    }

    public int GetMaxWinIndex()
    {
        return MathUtil.GetMaxIndexFromBetweenValues(minWin, maxWin);
    }

    public int GetRealPiecesToWinNeededFromIndex(int index)
    {
        return minSize + index;
    }

    public Vector2 GetFieldVec2FromIndex(int index)
    {
        return Vector2.one * (minSize + index);
    }
}
