using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiDifficultyButtonPanel : MonoBehaviour
{

    [SerializeField]
    private Button easyButton;

    [SerializeField]
    private Button hardButton;

    [SerializeField]
    private MenuHistoryManager menuController;

    [SerializeField]
    private GameObject gameSettings;

    [SerializeField]
    private Button backButton;

    private void Awake()
    {
        easyButton.onClick.AddListener(() =>
        {
            ButtonSetDifficulty(AIDifficulty.EASY);
        });

        hardButton.onClick.AddListener(() =>
        {
            ButtonSetDifficulty(AIDifficulty.HARD);
        });

        backButton.onClick.AddListener(() =>
        {
            menuController.CloseCurrentMenu();
        });
    }

    private void ButtonSetDifficulty(AIDifficulty difficulty)
    {
        GameSetupValues.INSTANCE.AiDifficulty = difficulty;
        menuController.OpenMenu(gameSettings);
    }

    

}
