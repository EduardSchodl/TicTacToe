using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HUD : MonoBehaviour
{
    public static NetworkManager manager;

    GameSettingScript gameSettingScript;

    [SerializeField]
    private GameObject gameSetting;

    [SerializeField]
    private GameObject nameOrCode;

    [SerializeField]
    private GameObject lobby;

    [SerializeField]
    private GameObject statusText;

    [SerializeField]
    private GameObject address;

    [SerializeField]
    private GameObject addressText;

    [SerializeField]
    private GameObject host;

    [SerializeField]
    private GameObject client;
    
    [SerializeField]
    private GameObject username;

    [SerializeField]
    private GameObject clientName;

    [SerializeField]
    private GameObject backButton;

    [SerializeField]
    private MenuHistoryManager menuController;

    void Awake()
    {
        manager = GameServerClientManager.INSTANCE;
        gameSettingScript = gameSetting.GetComponent<GameSettingScript>();
    }

    void OnGUI()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        // client ready
        if (NetworkClient.isConnected && !NetworkClient.ready)
        {
            if (GUILayout.Button("Client Ready"))
            {
                NetworkClient.Ready();
                if (NetworkClient.localPlayer == null)
                {
                    NetworkClient.AddPlayer();
                }
            }
        }

    }

    private void StartButtons()
    {
        if (!NetworkClient.active)
        {
            statusText.SetActive(false);
            
            ShowConnection();

            // Server + Client
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                host.GetComponent<Button>().onClick.AddListener(Host);
            }

            // Client + IP
            client.GetComponent<Button>().onClick.AddListener(Client);

            manager.networkAddress = addressText.GetComponent<TextMeshProUGUI>().text = (manager.networkAddress);

        }
        else
        {
            // Connecting
            statusText.SetActive(true);
            statusText.GetComponent<TextMeshProUGUI>().text = $"Connecting to {manager.networkAddress}..";
        }
    }


    private void StopHost()
    {
        statusText.SetActive(false);
        manager.StopHost();
    }

    private void StopClient()
    {
        statusText.SetActive(false);
        manager.StopClient();
    }

    private void StopServer()
    {
        statusText.SetActive(false);
        manager.StopServer();
    }

    private void StatusLabels()
    {
        statusText.SetActive(true);

        // host mode
        // display separately because this always confused people:
        //   Server: ...
        //   Client: ...
        if (NetworkServer.active && NetworkClient.active)
        {
            statusText.GetComponent<TextMeshProUGUI>().text = $"<b>Host</b>: running via {Transport.activeTransport}";
        }
        // server only
        else if (NetworkServer.active)
        {
            statusText.GetComponent<TextMeshProUGUI>().text = $"<b>Server</b>: running via {Transport.activeTransport}";
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            statusText.GetComponent<TextMeshProUGUI>().text = $"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}";
        }
    }

    private void ShowConnection()
    {
        HUDControl(true);
    }

    public void HideConnection()
    {
        HUDControl(false);
    }

    public void HUDControl(bool status)
    {
        host.SetActive(status);
        client.SetActive(status);
        address.SetActive(status);
        username.SetActive(status);
        backButton.SetActive(status);
    }

    public void CancelConnection()
    {
        statusText.SetActive(false);
        

        manager.StopClient();
    }

    public void Host()
    {
        GameSetupValues.INSTANCE.ServerSetHostName(clientName.GetComponent<TextMeshProUGUI>().text);

        menuController.OpenMenu(gameSetting);
    }

    public void Client()
    {
        GameSetupValues.INSTANCE.ServerSetClientName(clientName.GetComponent<TextMeshProUGUI>().text);
        manager.StopHost();
        
        GameServerClientManager.INSTANCE.SetPortToBasePort();

        manager.StartClient();
        
        

        menuController.OpenMenu(lobby);
    }

}
