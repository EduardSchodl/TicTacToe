using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(FieldAI))]
public class PlayingField : NetworkBehaviour, IOnDragStart, IOnDragEnd
{

    private static PlayingField instance;

    public static PlayingField INSTANCE => instance;

    

    public override void OnStartServer()
    {
        GameStateManager.INSTANCE.ActivePlayingField = this;
    }

    public override void OnStartClient()
    {
        GameStateManager.INSTANCE.ActivePlayingField = this;
        SpawnTiles();

        CenterCameraToPlayingField();
    }

    public void CenterCameraToPlayingField()
    {
        Camera mainCamera = Camera.main;

        Vector2 playingFieldPosition = transform.position;

        float cameraX = playingFieldPosition.x + Field.GetFieldMaxX() / 2f;
        float cameraY = playingFieldPosition.y + Field.GetFieldMaxY() / 2f;

        mainCamera.gameObject.transform.position = new Vector3(cameraX, cameraY, mainCamera.transform.position.z);
    }

    /// <summary>
    /// The GameObject prefab which represents a game tile.
    /// </summary>
    [SerializeField]
    private GameObject fieldTileP;

    private const int TileWidthHeight = 1;

    public Field Field
    {
        private set;
        get;
    } = new Field();

    #region Scripts
    private FieldAI fieldAI;
    #endregion

    private bool cameraStartedDragging = false;

    public bool ForcePlayerInputBlocked
    {
        get;
        set;
    } = false;

    public bool GameEnded
    {
        get;
        set;
    } = false;

    public bool IsPlayerInputBlocked()
    {
        return ForcePlayerInputBlocked || GameEnded;
    }

    void Awake()
    {
        instance = this;
        fieldAI = GetComponent<FieldAI>();

        DragScript cameraDrag = Camera.main.GetComponent<DragScript>();
        cameraDrag.AddOnDragStart(this);
        cameraDrag.AddOnDragEnd(this);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        CheckClickedTile();
    }

    public void CheckClickedTile()
    {
        if(IsPlayerInputBlocked())
        {
            return;
        }

        if(!Input.GetMouseButtonUp(0) || cameraStartedDragging)
        {
            return;
        }

        // To avoid clicking when the player is in the lobby
        // but the playing field remains in the game.
        // The best thing would be to destroy the PlayingField => then this could be removed.
        if(!GameStateManager.IsIngame())
        {
            return;
        }

        // Take all the objects under the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

        foreach(RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check whether these objects constitute tiles of this playing field.
            if (!Field.IsFieldsTile(hitObject))
            {
                continue;
            }

            FieldTile tile = hitObject.GetComponent<FieldTile>();
            FieldTileData tileData = Field.GetFieldTileDataByScript(tile);

            Player turnPlayer = NetPlayerManager.INSTANCE.GetMarkerForLocalPlayer();
            TryPlaceMarkerOnTile(turnPlayer, tileData, true);
            
        }
    }

    public bool PerformTilePlaceCheck(FieldTileData targetTile, Player markerOwner)
    {

        bool isOutOfTurn = TurnManager.INSTANCE.TurnPlayer != markerOwner;
        bool targetIsOccupied = targetTile.IsOccupied();
        bool shouldRollback = isOutOfTurn || targetIsOccupied;
        return shouldRollback;

    }

    /// <summary>
    /// Sets the sets the marker on the targetTilePos
    /// to the markerOwner if the targetTilePos is not occupied.
    /// </summary>
    [Server]
    public void ServerSetTileMarker(Vector2Int targetTilePos, Player markerOwner, bool humanPlaced)
    {
        FieldTileData targetTile = Field.TryGetTile(targetTilePos);
        
        NetworkConnection senderConnection = connectionToClient;

        bool shouldRollback = PerformTilePlaceCheck(targetTile, markerOwner);

        if (shouldRollback)
        {
            TargetRollbackTileMarker(senderConnection, targetTilePos);
            return;
        }

        LocalSetTileMarker(targetTilePos, markerOwner);

        TurnTimer.INSTANCE.ServerStartTimer(TurnTimer.TURN_TIME, false);

        // Server game end check
        var endCheckResult = EndChecker.CheckGameEndedFromTile(Field, targetTile);
        
        RpcSetTileMarker(targetTilePos, markerOwner, endCheckResult.gameEnded, endCheckResult.winningPlayer, humanPlaced);

        TurnManager.INSTANCE.ServerNextTurn();
    }

    [ClientRpc]
    public void RpcSetTileMarker(Vector2Int targetTilePos, Player markerOwner, bool gameEnded, Player? winningPlayer, bool humanPlaced)
    {
        LocalSetTileMarker(targetTilePos, markerOwner, true);

        if (gameEnded)
        {
            OnGameEnd(winningPlayer, targetTilePos);
            return;
        }

        // Advance turn, if humanPlaced, let ai play the next move
        NextTurnAi(humanPlaced && !gameEnded && GameSetupValues.INSTANCE.PlayingAgainstAi);


    }

    [TargetRpc]
    public void TargetRollbackTileMarker(NetworkConnection target, Vector2Int targetTilePos)
    {
        LocalSetTileMarker(targetTilePos, null);
    }

    /// <summary>
    /// Doesn't include isOccupied check.
    /// </summary>
    public void LocalSetTileMarker(Vector2Int targetTilePos, Player? markerOwner, bool useCheck = false)
    {
        FieldTileData targetTile = Field.TryGetTile(targetTilePos);

        if(useCheck && targetTile.IsOccupied())
        {
            return;
        }

        Field.SetPlayerForTile(targetTilePos, markerOwner, true);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetTileMarker(Vector2Int targetTilePos, Player markerOwner, bool humanPlaced)
    {
        ServerSetTileMarker(targetTilePos, markerOwner, humanPlaced);
    }

    private void TryPlaceMarkerOnTile(Player markerOwner, FieldTileData targetTile, bool humanPlaced)
    {
        // Can't place on occupied tiles
        if (PerformTilePlaceCheck(targetTile, markerOwner))
        {
            return;
        }

        // Sucessfully placed
        // Set the correct mark indicator and tile owner.
        Vector2Int targetTilePos = targetTile.TilePosition;

        if(!isServer)
        {
            LocalSetTileMarker(targetTilePos, markerOwner);
        }
        CmdSetTileMarker(targetTilePos, markerOwner, humanPlaced);

        /*
        // Advance turn, if humanPlaced, let ai play the next move
        NextTurnAi(humanPlaced && GameSetupValues.INSTANCE.PlayingAgainstAi);
        */

    }

    public void HighlightWinningTileStreak(Vector2Int lastPlacedTile)
    {
        FieldTileData lastPlacedTileData = Field.GetTile(lastPlacedTile);
        var endCheckResult = EndChecker.CheckGameEndedFromTile(Field, lastPlacedTileData);
        TileStreak winningStreak = endCheckResult.winningStreak;

        // Do nothing on draw
        if(winningStreak == null)
        {
            return;
        }

        foreach(FieldTileData tile in winningStreak.IterateStreakTiles())
        {
            tile.TileScript.TurnOnTileHalo();
        }
    }

    public void OnGameEnd(Player? winningPlayer, Vector2Int lastPlacedTile)
    {
        GameEnded = true;

        HighlightWinningTileStreak(lastPlacedTile);

        MatchInfo matchInfo = MatchInfo.INSTANCE;

        matchInfo.SetLastWinningPlayer(winningPlayer);
        matchInfo.IncreaseWinCountFor(winningPlayer);
        
        IngameUICanvas.INSTANCE.OnGameEnd(winningPlayer);
    }

    public void NextTurnAi(bool letAiPlayNextMove)
    {
        if(!letAiPlayNextMove)
        {
            return;
        }

        StartAiTurn();
    }

    public void StartAiTurn()
    {
        fieldAI.StartAIComputation(Field);
    }

    public void AiPlaceMarkerOnTile(Vector2Int tilePosition)
    {
        Player turnPlayer = TurnManager.INSTANCE.TurnPlayer;
        TryPlaceMarkerOnTile(turnPlayer, Field.GetTile(tilePosition), false);
    }

    public void SpawnTiles()
    {
        GameSetupValues gameSetupValues = GameSetupValues.INSTANCE;
        Vector2Int playingFieldDimensions = gameSetupValues.FieldSize;

        for(int y = 0; y < playingFieldDimensions.y; ++y)
        {
            for(int x = 0; x < playingFieldDimensions.y; ++x)
            {
                SpawnTile(new Vector2Int(x, y));
            }
        }
    }

    /// <summary>
    /// Spawns a tile <see cref="GameObject"/> on the specified position
    /// and adds a <see cref="FieldTileData"/> entry into this <see cref="PlayingField"/>s
    /// <see cref="Field.Tiles"/> dictionary.
    /// </summary>
    /// <param name="fieldPosition"></param>
    private void SpawnTile(Vector2Int fieldPosition)
    {
        GameSetupValues gameSetupValues = GameSetupValues.INSTANCE;
        Vector2Int playingFieldDimensions = gameSetupValues.FieldSize;

        // Instantiate a new tile
        GameObject fieldTileObj = Instantiate(fieldTileP);
        FieldTile fieldTile = fieldTileObj.GetComponent<FieldTile>();

        // Add a new entry on the fieldPosition initialzed with the new fieldTile script.
        Field.AddTile(fieldPosition, fieldTile);

        // Set the correct position.
        fieldTileObj.transform.parent = gameObject.transform;
        Vector2 multipl = fieldPosition * TileWidthHeight;
        fieldTileObj.transform.position = multipl;

        // Set the correct sprite.
        fieldTile.UpdateTileSprite(fieldPosition, playingFieldDimensions);
    }

    public void OnDragStart()
    {
        cameraStartedDragging = true;
    }

    public void OnDragEnd()
    {
        cameraStartedDragging = false;
    }
}
