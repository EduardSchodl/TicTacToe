using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
    private static LobbyScript instance;
    public static LobbyScript INSTANCE => instance;

    public static bool IsInLobby()
    {
        return INSTANCE != null && INSTANCE.gameObject.activeInHierarchy;
    }
    


    [SerializeField]
    private GameObject fieldSize;

    [SerializeField]
    private GameObject piecesNeededToWin;

    [SerializeField]
    private GameObject numOfMatches;

    [SerializeField]
    private Button ready;

    [SerializeField]
    private GameObject hostName;

    [SerializeField]
    private GameObject clientName;

    [SerializeField]
    private Toggle hostToggle;

    [SerializeField]
    private Toggle clientToggle;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError($"Tried to create a second instance of {nameof(LobbyScript)}.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        ready.onClick.AddListener(OnReadyClick);
    }

    public void OnReadyClick()
    {
        NetPlayerManager.INSTANCE.GetLocalPlayer().CmdSetIsLobbyReady(true);
    }

    public void UpdateReadyGUI()
    {
        PlayerScr hostPlayer = NetPlayerManager.INSTANCE.HostPlayer;
        PlayerScr clientPlayer = NetPlayerManager.INSTANCE.ClientPlayer;

        if (hostPlayer != null)
        {
            hostToggle.isOn = NetPlayerManager.INSTANCE.HostPlayer.IsLobbyReady;
        }

        if(clientPlayer != null)
        {
            clientToggle.isOn = NetPlayerManager.INSTANCE.ClientPlayer.IsLobbyReady;
        }
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        UpdateValues();

        if(Input.GetKey(KeyCode.X))
        {
            Debug.Log(GameSetupValues.INSTANCE.ClientName);
        }
    }

    private void UpdateValues()
    {
        fieldSize.GetComponent<TextMeshProUGUI>().text = GameSetupValues.INSTANCE.FieldSize.ToString();
        piecesNeededToWin.GetComponent<TextMeshProUGUI>().text = GameSetupValues.INSTANCE.PiecesNeededToWin.ToString();
        numOfMatches.GetComponent<TextMeshProUGUI>().text = GameSetupValues.INSTANCE.TotalRoundsPlayed.ToString();

        hostName.GetComponent<TextMeshProUGUI>().text = GameSetupValues.INSTANCE.HostName;

        if (GameSetupValues.INSTANCE.ClientName != null)
        {
            clientName.GetComponent<TextMeshProUGUI>().text = GameSetupValues.INSTANCE.ClientName;
        }
    }

    private void OnEnable()
    {
        Debug.Log("i was enabled");
    }

}
