using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FieldAI : MonoBehaviour
{

    private PlayingField playingField;

    private void Awake()
    {
        playingField = GetComponent<PlayingField>();
    }

    /// <summary>
    /// Returns the best possible move for the turn player
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public void StartAIComputation(Field field)
    {
        TurnManager turnManager = TurnManager.INSTANCE;
        Player turnPlayer = turnManager.TurnPlayer;
        
        // Root node copies the state of the game
        // turnPlayer is the next player at this point so the
        // last placing player is the previous one for the root state.
        
        Player previousPlayer = turnPlayer.GetPreviousCircular();

        FieldBestMoveJob fieldBestMoveJob = new FieldBestMoveJob(field, previousPlayer);
        fieldBestMoveJob.AddOnJobStartHook(AiComputationStart);
        fieldBestMoveJob.AddOnJobEndHook(AiComputationEnd);
        
        fieldBestMoveJob.StartJob();
    }
    
    private void AiComputationStart()
    {
        lock(playingField)
        {
            playingField.ForcePlayerInputBlocked = true;
        }
    }

    private void AiComputationEnd(Vector2Int bestMove)
    {
        lock(playingField)
        {
            playingField.ForcePlayerInputBlocked = false;
            playingField.AiPlaceMarkerOnTile(bestMove);
        }
    }    

}
