using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerCounterIntoVector : MonoBehaviour, IPickerIndexToTextTransformer
{
    public string GetTextFromPickerIndex(int pickerCounter)
    {
        Vector2Int vec = Vector2Int.one * pickerCounter;
        return VectorUtil.VectorToFieldString(vec);
    }
}
