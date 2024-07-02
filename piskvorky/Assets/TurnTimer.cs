using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;

public class TurnTimer : NetworkBehaviour
{

    public const double GAME_END_TIME = 15.0;
    public const double TURN_TIME = 10.0;

    private bool timerEndGameMode = false;

    [SerializeField]
    private TextMeshProUGUI timerValueText;

    [SerializeField]
    private double startCountdownTime = 30.0;

    private double countDownTime;

    public static TurnTimer INSTANCE
    {
        get;
        private set;
    }


    private void Awake()
    {
        INSTANCE = this;

        ResetTimer();

    }

    public override void OnStartClient()
    {
        if (!GameSetupValues.INSTANCE.IsOnline)
        {
            gameObject.SetActive(false);
        }
    }

    private bool timeIsRunning = false;

    
    // Update is called once per frame
    void Update()
    {
        
        if (timeIsRunning)
        {
            countDownTime -= Time.deltaTime;

            timerValueText.text = (Mathf.Round((float)countDownTime * 100.0f) / 100.0f).ToString();

            if (countDownTime < 0)
            {
                timerValueText.text = "0";
                timeIsRunning = false;

                GameOverFromTimer();
            }
        }
    }
    
    public void LocalStartTimer()
    {
        timeIsRunning = true;
    }

    public void StopTimer()
    {
        timeIsRunning = false;
    }
    
    [Server]
    public void ServerStartTimer(double startTime, bool timerEndGameMode)
    {
        RpcStartTimer(startTime, NetworkTime.time, timerEndGameMode);
    }

    [ClientRpc]
    public void RpcStartTimer(double startTime, double serverStartTime, bool timerEndGameMode)
    {
        this.timerEndGameMode = timerEndGameMode;

        double currentTime = NetworkTime.time;
        double difference = currentTime - serverStartTime;

        double realStartTime = startTime - difference;

        countDownTime = Mathf.Round((float)realStartTime * 100.0f) / 100.0f;
        LocalStartTimer();
    }

    public void ResetTimer()
    {
        countDownTime = startCountdownTime;
    }

    public  void SetTimerTime(float countDownTime)
    {
        startCountdownTime = countDownTime;
    }

    [Server]
    public void ServerMakePlayerLoseTimerEnd()
    {
        RpcTimerOver();
    }

    [ClientRpc]
    public void RpcTimerOver()
    {

        MatchInfo matchInfo = MatchInfo.INSTANCE;

        Player winningPlayer = TurnManager.INSTANCE.TurnPlayer.GetNextCircular();

        matchInfo.SetLastWinningPlayer(winningPlayer);
        matchInfo.IncreaseWinCountFor(winningPlayer);

        PlayingField.INSTANCE.GameEnded = true;
        IngameUICanvas.INSTANCE.OnGameEnd(winningPlayer);

    }

    //Po odkomentování se pøièítají body, ale neupdatuje text
    //pouze na hostovi
    private void GameOverFromTimer()
    {
        GameStateManager manager = GameStateManager.INSTANCE;
        if (!manager.isServer)
        {
            return;
        }

        if(!timerEndGameMode)
        {
            ServerMakePlayerLoseTimerEnd();
            return;
        }

        GameStateManager.INSTANCE.HandleNextRoundCycle();

    }
}
