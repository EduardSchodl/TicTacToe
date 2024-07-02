using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CounterComponent))]
public class MatchCounterComponent : MonoBehaviour
{
    private CounterComponent counterComponent;

    private void Awake()
    {
        counterComponent = GetComponent<CounterComponent>();
    }

    private void Start()
    {
        UpdateCounterComponentFromMatches();
    }

    public void UpdateCounterComponentFromMatches()
    {
        int totalMatchesPlayed = MatchInfo.INSTANCE.GetTotalMatchesPlayed();
        counterComponent.SetCounterNumber(totalMatchesPlayed);
    }
}
