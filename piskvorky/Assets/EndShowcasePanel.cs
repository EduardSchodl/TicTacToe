using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndShowcasePanel : MonoBehaviour
{

    [SerializeField]
    private Button exitShowcaseButton;

    [SerializeField]
    private WinLoseElement matchWinLoseElement;

    [SerializeField]
    private GameObject persistentPanel;

    public void OnSwitchToShowcase()
    {
        Player? matchWinningPlayer = MatchInfo.INSTANCE.GetMatchWinningPlayer();
        Debug.Log("matchWinningPlayer " + matchWinningPlayer);
        matchWinLoseElement.UpdateElement(matchWinningPlayer);

        persistentPanel.SetActive(false);
    }

    private void Awake()
    {
        exitShowcaseButton.onClick.AddListener(OnExitShowcaseButtonClick);
    }

    public void OnExitShowcaseButtonClick()
    {
        GameSceneManager.INSTANCE.GameSceneToGUI();
    }

}
