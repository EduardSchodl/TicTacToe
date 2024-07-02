using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtFieldSizeIntoIndex : MonoBehaviour, IPickerExtValueIntoIndex
{
    [SerializeField]
    private GameSetupSettingValues pickerSettings;

    public int GetPickerIndexFromExtValue()
    {
        Vector2Int fieldSize = GameSetupValues.INSTANCE.FieldSize;
        int index = fieldSize.x - pickerSettings.MinSize;
        return index;
    }

}
