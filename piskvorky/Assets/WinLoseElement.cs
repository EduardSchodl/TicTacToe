using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WinLoseElement : MonoBehaviour
{
    // Subjective win sprites (ai game)
    [SerializeField]
    private Sprite winSprite;
    [SerializeField]
    private Sprite loseSprite;
    
    // Objective win sprites (local pvp + network game)
    [SerializeField]
    private Sprite xWin;
    [SerializeField]
    private Sprite oWin;

    [SerializeField]
    private Sprite drawSprite;


    private Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }


    public Sprite PickObjectWinLoseSprite(Player winningPlayer)
    {
        if(winningPlayer == Player.CIRCLE)
        {
            return oWin;
        }
        else if(winningPlayer == Player.CROSS)
        {
            return xWin;
        }

        Debug.LogError($"Tried to pick a win lose sprite for a non-supported player {winningPlayer}.");
        return oWin;
    }

    public Sprite PickSubjectiveWinLoseSprite(Player winningPlayer)
    {
        
        bool weWon = MatchInfo.INSTANCE.PlayerIsMyPlayer(winningPlayer);
        Sprite pickedSprite = weWon ? winSprite : loseSprite;
        return pickedSprite;
    }

    public Sprite PickWinLoseSprite(Player? winningPlayer)
    {
        if(!winningPlayer.HasValue)
        {
            return drawSprite;
        }

        return GameSetupValues.INSTANCE.PlayingAgainstAi
            ? PickSubjectiveWinLoseSprite(winningPlayer.Value) : PickObjectWinLoseSprite(winningPlayer.Value);
    }

    public void UpdateElement(Player? winningPlayer)
    {
        
        imageComponent.sprite = PickWinLoseSprite(winningPlayer);
    }

}
