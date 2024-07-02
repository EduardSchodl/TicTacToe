using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounterComponent : MonoBehaviour
{
    [SerializeField]
    private CounterComponent leftCounter;
    [SerializeField]
    private CounterComponent rightCounter;

    public void UpdateCounterComponentFromMatchInfo()
    {
        MatchInfo matchInfo = MatchInfo.INSTANCE;
        int myWins = matchInfo.GetMyWins();
        int otherWins = matchInfo.GetOtherWins();

        leftCounter.SetCounterNumber(myWins);
        rightCounter.SetCounterNumber(otherWins);
    }

    private void Awake()
    {
        StaticPlayerInfo staticInfo = StaticPlayerInfo.INSTANCE;
        leftCounter.CounterDigitColor = staticInfo.GetPlayerInfoByID(Player.CIRCLE).PlayerColor;
        rightCounter.CounterDigitColor = staticInfo.GetPlayerInfoByID(Player.CROSS).PlayerColor;
    }

    private void Start()
    {
        UpdateCounterComponentFromMatchInfo();
    }

}
