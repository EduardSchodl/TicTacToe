using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtMatchCountIntoIndex : MonoBehaviour, IPickerExtValueIntoIndex
{
    [SerializeField]
    private GameSetupSettingValues pickerSettings;

    public int GetPickerIndexFromExtValue()
    {
        return GameSetupValues.INSTANCE.TotalRoundsPlayed - pickerSettings.MinMatchCount;
    }

}
