using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtPiecesToIndex : MonoBehaviour, IPickerExtValueIntoIndex
{
    [SerializeField]
    private GameSetupSettingValues pickerSettings;

    public int GetPickerIndexFromExtValue()
    {
        return GameSetupValues.INSTANCE.PiecesNeededToWin - pickerSettings.MinWin;
    }

}
