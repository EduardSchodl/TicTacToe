using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPanel : MonoBehaviour
{

    [SerializeField]
    private WinLoseElement winLoseElement;

    [SerializeField]
    private GameEndButton bottomButton;

    public void OnGameEnd(Player? winningPlayer)
    {
        winLoseElement.UpdateElement(winningPlayer);
        bottomButton.UpdateText();
    }

}
