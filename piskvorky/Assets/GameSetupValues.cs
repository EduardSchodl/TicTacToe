using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSetupValues : NetworkBehaviour
{
    public const string HOST_DEFAULT_NAME = "HOST";
    public const string CLIENT_DEFAULT_NAME = "CLIENT";

    private Vector2Int DEFAULT_FIELD_SIZE = new Vector2Int(3, 3);
    private const int DEFAULT_PIECES_NEEDED_TO_WIN = 3;
    private const int DEFAULT_TOTAL_ROUNDS_PLAYED = 1;
    private const AIDifficulty DEFAULT_AI_DIFFICULTY = AIDifficulty.EASY;

    [SyncVar(hook = nameof(SyncFieldSize))]
    private Vector2Int fieldSize;

    [SyncVar(hook = nameof(SyncPiecesNeededToWin))]
    private int piecesNeededToWin;

    [SyncVar(hook = nameof(SyncTotalRoundsPlayed))]
    private int totalRoundsPlayed;

    [SyncVar(hook = nameof(SyncHostName))]
    private string hostName;

    [SyncVar(hook = nameof(SyncClientName))]
    private string clientName;

    public Vector2Int FieldSize => fieldSize;
    public int PiecesNeededToWin => piecesNeededToWin;
    public int TotalRoundsPlayed => totalRoundsPlayed;
    public string HostName => hostName;
    public string ClientName => clientName;


    [Server]
    public void ServerResetValues()
    {
        ServerSetFieldSize(DEFAULT_FIELD_SIZE);
        ServerSetPiecesNeededToWin(DEFAULT_PIECES_NEEDED_TO_WIN);
        ServerSetTotalRoundsPlayed(DEFAULT_TOTAL_ROUNDS_PLAYED);
        IsOnline = false;
        AiDifficulty = DEFAULT_AI_DIFFICULTY;
        ServerSetHostName(HOST_DEFAULT_NAME);
        ServerSetClientName(CLIENT_DEFAULT_NAME);

        Debug.Log("manager: " + NetPlayerManager.INSTANCE);
        Debug.Log("FF " + NetPlayerManager.INSTANCE.HostPlayer);
        NetPlayerManager.INSTANCE.HostPlayer.ServerSetIsLobbyReady(false);
    }

    private void SyncFieldSize(Vector2Int oldValue, Vector2Int newValue)
    {
        this.fieldSize = newValue;
    }

    private void SyncPiecesNeededToWin(int oldValue, int newValue)
    {
        this.piecesNeededToWin = newValue;
    }

    private void SyncTotalRoundsPlayed(int oldValue, int newValue)
    {
        this.totalRoundsPlayed = newValue;
    }

    private void SyncHostName(string oldValue, string newValue)
    {
        this.hostName = newValue;
    }

    private void SyncClientName(string oldValue, string newValue)
    {
        this.clientName = newValue;
    }

    [Server]
    public void ServerSetFieldSize(Vector2Int newValue)
    {
        SyncFieldSize(FieldSize, newValue);
    }

    [Server]
    public void ServerSetPiecesNeededToWin(int newValue)
    {
        SyncPiecesNeededToWin(PiecesNeededToWin, newValue);
    }

    [Server]
    public void ServerSetTotalRoundsPlayed(int newValue)
    {
        SyncTotalRoundsPlayed(TotalRoundsPlayed, newValue);
    }

    [Server]
    public void ServerSetHostName(string newValue)
    {
        SyncHostName(HostName, newValue);
    }

    [Server]
    public void ServerSetClientName(string newValue)
    {
        SyncClientName(ClientName, newValue);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetClientName(string newValue)
    {
        SyncClientName(ClientName, newValue);
    }

    public bool PlayingAgainstAi
    {
        get;
        set;
    }

    public AIDifficulty AiDifficulty
    {
        get;
        set;
    } = AIDifficulty.EASY;

    public static GameSetupValues INSTANCE
    {
        get;
        private set;
    }
    
    public bool IsOnline
    {
        get;
        set;
    }
    private void OnDestroy()
    {
        INSTANCE = null;
    }


    private void Awake()
    {
        if(INSTANCE != null)
        {
            Debug.LogError($"Tried to create a second {nameof(GameSetupValues)} object.");
            return;
        }

        INSTANCE = this;

        DontDestroyOnLoad(gameObject);
    }

}
