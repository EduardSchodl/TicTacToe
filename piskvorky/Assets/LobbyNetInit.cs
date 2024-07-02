using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyNetInit : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clientTextField;

    public override void OnStartClient()
    {
        if (!isServer)
        {
            OnStartClientServerJoin();
        }
    }

    [Client]
    private void OnStartClientServerJoin()
    {
        GameSetupValues.INSTANCE.CmdSetClientName(clientTextField.text);
    }

    [Client]
    public void ClientSentLobbyValuesToServer()
    {
        
    }
}
