using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameUICanvas : MonoBehaviour
{
    public static IngameUICanvas INSTANCE
    {
        private set;
        get;
    }

    [SerializeField]
    private GameObject ingameOverlayPanel;

    [SerializeField]
    private GameEndPanel gameEndPanel;

    [SerializeField]
    private ScoreCounterComponent scoreCounter;

    [SerializeField]
    private MatchCounterComponent matchCounter;

    [SerializeField]
    private EndShowcasePanel endShowcasePanel;

    [SerializeField]
    private GameObject escapePanel;

    private bool escapePanelFullyHidden = false;

    private void Update()
    {

        if(!GameSetupValues.INSTANCE.IsOnline && !escapePanelFullyHidden && Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchEscapePanel();
        }

    }

    public void FullyHideEscapePanel()
    {
        escapePanelFullyHidden = true;
        escapePanel.SetActive(false);
    }    

    public void SwitchEscapePanel()
    {
        escapePanel.SetActive(!escapePanel.activeInHierarchy);
    }

    public void OnGameEnd(Player? winningPlayer)
    {
        ShowGameEndPanel(true);
        gameEndPanel.OnGameEnd(winningPlayer);
        scoreCounter.UpdateCounterComponentFromMatchInfo();

        matchCounter.UpdateCounterComponentFromMatches();

        
        TurnTimer timer = TurnTimer.INSTANCE;

        if(timer.isServer)
        {
            timer.ServerStartTimer(TurnTimer.GAME_END_TIME, true);
        }
        
    }

    public void OnGameStart()
    {
        ShowGameEndPanel(false);
    }

    public void ShowGameEndPanel(bool gameEnded)
    {
        ingameOverlayPanel.SetActive(!gameEnded);
        gameEndPanel.gameObject.SetActive(gameEnded);
    }

    public void ShowEndgameShowcase()
    {
        ingameOverlayPanel.SetActive(false);
        gameEndPanel.gameObject.SetActive(false);
        endShowcasePanel.gameObject.SetActive(true);

        endShowcasePanel.OnSwitchToShowcase();
    }

    private void Awake()
    {
        if(INSTANCE != null)
        {
            Debug.LogError($"Tried to create a second instance of {nameof(IngameUICanvas)}");
            return;
        }
        INSTANCE = this;
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

}
