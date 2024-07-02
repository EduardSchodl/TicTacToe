using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject playingFieldP;

    public PlayingField ActivePlayingField
    {
        get;
        set;
    }

    public static bool IsIngame()
    {
        return GameSceneManager.IsSceneLoaded(SceneNames.GAME_SCENE);
    }

    public static GameStateManager INSTANCE
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(INSTANCE != null)
        {
            Debug.LogError($"Tried to create a second {nameof(GameStateManager)}");
            return;
        }

        DontDestroyOnLoad(gameObject);
        INSTANCE = this;
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

    [Server]
    public void ServerNextMatchRound()
    {
        TurnManager.INSTANCE.ServerSetTurnPlayerToLoser();
        ServerCreateAndReplaceNewPlayingField();
        RpcIngameUIOnGameStart();
    }

    [ClientRpc]
    public void RpcIngameUIOnGameStart()
    {
        IngameUICanvas.INSTANCE.OnGameStart();
    }

    public void HandleNextRoundCycle()
    {
        bool matchEnded = MatchInfo.INSTANCE.MatchHasEnded();

        if (!matchEnded)
        {
            NextMatchRound();
            return;
        }

        GoToEndShowcase();
    }

    public void NextMatchRound()
    {
        if(isServer)
        {
            ServerNextMatchRound();
        }
    }

    public void OnFirstRoundHook()
    {

        if(isServer)
        {
            TurnManager.INSTANCE.ServerSetTurnPlayerToRandom();
        }

    }

    
    public void GoToEndShowcase()
    {
        
        if(isServer)
        {
            ServerEndGame();
        }

    }

    [Server]
    public void ServerEndGame()
    {
        RpcDisconnectClient();
    }

    [ClientRpc]
    public void RpcDisconnectClient()
    {
        IngameUICanvas.INSTANCE.ShowEndgameShowcase();

        if (isServer)
        {
            return;
        }

        GameServerClientManager.INSTANCE.StopAndStartClient();
    }

    public void DestroyPlayingField()
    {
        Destroy(ActivePlayingField.gameObject);
    }

    [Server]
    public void ServerCreateAndReplaceNewPlayingField()
    {

        // Destroy old playing field
        if (ActivePlayingField != null)
        {
            DestroyPlayingField();
        }
        
        // Create new playing field
        GameObject playingFieldObj = Instantiate(playingFieldP);
        PlayingField playingField = playingFieldObj.GetComponent<PlayingField>();
        
        NetworkServer.Spawn(playingFieldObj);

        TurnTimer.INSTANCE.ServerStartTimer(TurnTimer.TURN_TIME, false);   

        //ActivePlayingField = playingField;
    }

    [ClientRpc]
    public void RpcLoadGameScene()
    {
        LoadGameScene();
    }

    public void LoadGameScene()
    {
        AsyncOperation unloadResult = SceneManager.UnloadSceneAsync(SceneNames.GUI_SCENE);
        unloadResult.completed += OnGUISceneUnloaded;
    }

    private void OnGameSceneLoaded(AsyncOperation obj)
    {

        // Server will handle creation of the playing field
        // The client will receive the object from the server
        if (isServer)
        {
            ServerCreateAndReplaceNewPlayingField();
        }
        
        OnFirstRoundHook();
    }

    private void OnGUISceneUnloaded(AsyncOperation obj)
    {
        AsyncOperation loadResult = SceneManager.LoadSceneAsync(SceneNames.GAME_SCENE, LoadSceneMode.Additive);
        loadResult.completed += OnGameSceneLoaded;

    }
}
