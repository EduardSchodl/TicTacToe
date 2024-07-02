using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject networkManager;

    [SerializeField]
    private GameObject gameSettingApplyButton;

    [SerializeField]
    private GameObject gameSettingBackButton;

    [SerializeField]
    private MenuHistoryManager menuController;

    [SerializeField]
    private GameObject gameSetting;

    [SerializeField]
    private GameObject nameOrCode;

    [SerializeField]
    private GameObject aiDifficultyPanel;

    public void SetValuesFromMenu(bool isOnline, bool playingAgainstAi)
    {
        GameSetupValues.INSTANCE.IsOnline = isOnline;
        GameSetupValues.INSTANCE.PlayingAgainstAi = playingAgainstAi;
    }

    private void OpenLocalGameSettings()
    {
        menuController.OpenMenu(gameSetting);
        gameSettingApplyButton.SetActive(true);
        gameSettingBackButton.SetActive(true);
    }

    /// <summary>
    /// When you press the VS PC
    /// button.
    /// </summary>
    public void AiGameSetting()
    {
        SetValuesFromMenu(false, true);

        // Open the difficulty panel first
        menuController.OpenMenu(aiDifficultyPanel);
        
    }

    public void LocalGameSetting()
    {
        SetValuesFromMenu(false, false);
        OpenLocalGameSettings();
    }

    public void OnlineGameSetting()
    {
        SetValuesFromMenu(true, false);
        menuController.OpenMenu(nameOrCode);
    }

    public void BackMenu()
    {
        menuController.CloseCurrentMenu();
    }

    private void OnEnable()
    {
        ResetLobbyValues();
    }

    private void ResetLobbyValues()
    {
        GameSetupValues.INSTANCE.ServerResetValues();
    }
}
