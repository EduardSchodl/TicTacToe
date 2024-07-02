using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(GameSetupSettingValues))]
public class GameSettingScript : MonoBehaviour
{
    [SerializeField]
    private MenuHistoryManager menuController;

    private GameSetupSettingValues gameSetupVals;

    [SerializeField]
    private Button applyButton;

    [SerializeField]
    private ButtonPickerComponent fieldSizePicker;
    [SerializeField]
    private ButtonPickerComponent winPiecesPicker;
    [SerializeField]
    private ButtonPickerComponent matchCountPicker;

    [SerializeField]
    private GameObject lobby;

    [SerializeField]
    private GameObject nameOrCode;

    private void Awake()
    {
        gameSetupVals = GetComponent<GameSetupSettingValues>();

        fieldSizePicker.MinValue = gameSetupVals.MinSize;
        fieldSizePicker.MaxValue = gameSetupVals.MaxSize;

        winPiecesPicker.MinValue = gameSetupVals.MinWin;
        winPiecesPicker.MaxValue = gameSetupVals.MaxWin;

        matchCountPicker.MinValue = gameSetupVals.MinMatchCount;
        matchCountPicker.MaxValue = gameSetupVals.MaxMatchCount;

        applyButton.onClick.AddListener(OnApplyButtonClick);

    }

    public void OnApplyButtonClick()
    {
        if(GameSetupValues.INSTANCE.IsOnline)
        {
            OpenLobby();
            return;
        }
        ApplyGameSetting();
    }

    private void SetSetupValues()
    {
        GameSetupValues.INSTANCE.ServerSetFieldSize(VectorUtil.CounterToVector(fieldSizePicker.Iterator));
        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(winPiecesPicker.Iterator);
        GameSetupValues.INSTANCE.ServerSetTotalRoundsPlayed(matchCountPicker.Iterator);
    }

    public void ApplyGameSetting()
    {
        SetSetupValues();

        GameStateManager.INSTANCE.RpcLoadGameScene();
    }

    public void OpenLobby()
    {
        SetSetupValues();

        menuController.OpenMenu(lobby);
    }

    public void BackMenu()
    {
        menuController.CloseCurrentMenu();
    }
}
