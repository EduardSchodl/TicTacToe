using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Net.Sockets;
using kcp2k;

public class GameServerClientManager : NetworkManager
{
    private ushort basePort;
    public ushort BasePort => basePort;

    [SerializeField]
    private bool runningAsDebugClient;

    /// <summary>
    /// Called on the client when the
    /// Server decides that the game has ended.
    /// </summary>
    public void StopAndStartClient()
    {
        StopClient();
        if(runningAsDebugClient)
        {
            SetPortToDebugClientPort();
        }
        StartHost();
    }

    public static GameServerClientManager INSTANCE
    {
        private set;
        get;
    }

    public void SetPortToBasePort()
    {
        SetPort(basePort);
    }

    public void SetPortToDebugClientPort()
    {
        SetPort((ushort)(BasePort + 1));
    }

    public void SetPort(ushort val)
    {
        GetComponent<KcpTransport>().Port = val;
    }

    public ushort GetCurrentPort()
    {
        return GetComponent<KcpTransport>().Port;
    }

    public override void Awake()
    {
        base.Awake();

        basePort = GetComponent<KcpTransport>().Port;
        if(runningAsDebugClient)
        {
            Debug.Log("Will run local server on +1 client port.");
            SetPortToDebugClientPort();
        }

        INSTANCE = this;
    }

    public override void Start()
    {
        base.Start();

        Debug.Log($"Starting local server on port {GetCurrentPort()}");
        StartHost();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("OnServerDisconnect");
        base.OnServerDisconnect(conn);

        OnPlayerDisconnect();
    }

    public void OnPlayerDisconnect()
    {
        if (GameStateManager.IsIngame())
        {
            IngameUICanvas.INSTANCE.ShowEndgameShowcase();
        }

    }

    public override void OnClientDisconnect()
    {
        Debug.Log("OnClientDisconnect");
        base.OnClientDisconnect();
        OnPlayerDisconnect();

        if(LobbyScript.IsInLobby())
        {
            StopAndStartClient();
            MenuHistoryManager.INSTANCE.CloseCurrentMenu();
        }
        
    }


}
