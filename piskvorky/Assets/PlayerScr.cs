using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class PlayerScr : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncIsLobbyReady))]
    private bool isLobbyReady;

    public bool IsLobbyReady => isLobbyReady;

    [SyncVar(hook = nameof(SyncGamePlayer))]
    private Player gamePlayer;
    
    public Player GamePlayer => gamePlayer;

    public override void OnStopServer()
    {
        base.OnStopServer();

        ServerSetIsLobbyReady(false);
        NetPlayerManager.INSTANCE.RemoveNameForPlayer(this);
    }

    public void SyncGamePlayer(Player oldValue, Player newValue)
    {
        gamePlayer = newValue;
        TurnManager.INSTANCE.UpdateTurnComponent();

        /*
        bool isClient = NetPlayerManager.INSTANCE.ClientPlayer == this;
        if(!isClient)
        {
            return;
        }
        Debug.Log("CLIENT PLAYER: Syncing GamePlayer to " + newValue);
        */
    }    

    public void ServerSetGamePlayer(Player newValue)
    {
        SyncGamePlayer(gamePlayer, newValue);
    }

    public void SyncIsLobbyReady(bool oldValue, bool newValue)
    {
        isLobbyReady = newValue;


        // No gui to update
        if(LobbyScript.INSTANCE == null)
        {
            return;
        }

        // Update gui
        LobbyScript.INSTANCE.UpdateReadyGUI();
    }

    [Server]
    public void ServerSetIsLobbyReady(bool isLobbyReady)
    {
        SyncIsLobbyReady(this.isLobbyReady, isLobbyReady);


        // Server check for both players being ready
        if(NetPlayerManager.INSTANCE.AreBothPlayersLobbyReady())
        {
            GameStateManager.INSTANCE.RpcLoadGameScene();
        }

    }
    
    [Command]
    public void CmdSetIsLobbyReady(bool isLobbyReady)
    {
        ServerSetIsLobbyReady(isLobbyReady);
    }

    public override void OnStartClient()
    {
        InitPlayerToManager();

        if(LobbyScript.INSTANCE != null)
        {
            LobbyScript.INSTANCE.UpdateReadyGUI();
        }
    }
    
    private void InitPlayerToManager()
    {

        if (!isClient)
        {
            Debug.LogError("Tried to add player to manager from non-networked context.");
            return;
        }

        if(isServer)
        {
            HostPlayerInit();
            return;
        }

        ClientPlayerInit();
    }

    private void HostPlayerInit()
    {
        if(isLocalPlayer)
        {
            NetPlayerManager.INSTANCE.HostPlayer = this;
            return;
        }

        NetPlayerManager.INSTANCE.ClientPlayer = this;
    }

    private void ClientPlayerInit()
    {
        if(isLocalPlayer)
        {
            NetPlayerManager.INSTANCE.ClientPlayer = this;
            return;
        }

        NetPlayerManager.INSTANCE.HostPlayer = this;

    }

}
