using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetPlayerManager : MonoBehaviour
{
    public PlayerScr HostPlayer
    {
        get;
        set;
    }

    public PlayerScr ClientPlayer
    {
        get;
        set;
    }

    public void RemoveNameForPlayer(PlayerScr player)
    {
        GameSetupValues instance = GameSetupValues.INSTANCE;
        if (player == HostPlayer)
        {
            instance.ServerSetHostName(GameSetupValues.HOST_DEFAULT_NAME);
        }
        else if(player == ClientPlayer)
        {
            instance.ServerSetClientName(GameSetupValues.CLIENT_DEFAULT_NAME);
        }

    }

    public Player GetMarkerForLocalPlayer()
    {
        if(!IsClientConnected())
        {
            return TurnManager.INSTANCE.TurnPlayer;
        }

        PlayerScr localPlayer = GetLocalPlayer();
        return localPlayer.GamePlayer;
    }

    public bool IsClientConnected()
    {
        return ClientPlayer != null;
    }

    public bool AreBothPlayersLobbyReady()
    {
        return IsClientConnected() && (HostPlayer.IsLobbyReady && ClientPlayer.IsLobbyReady);
    }

    private static NetPlayerManager instance;
    public static NetPlayerManager INSTANCE => instance;

    public PlayerScr GetLocalPlayer()
    {
        if(HostPlayer.isLocalPlayer)
        {
            return HostPlayer;
        }

        if(ClientPlayer.isLocalPlayer)
        {
            return ClientPlayer;
        }

        Debug.LogError($"Did not find any player which would be returned in {nameof(GetLocalPlayer)}");
        return null;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning($"Tried to create a second {nameof(NetPlayerManager)}");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    

}
