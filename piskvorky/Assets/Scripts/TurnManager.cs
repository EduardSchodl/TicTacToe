using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : NetworkBehaviour
{
    private static TurnManager instance;

    [SerializeField]
    private TextMeshProUGUI turn;


    [SerializeField]
    private GameObject myPlayerImage;
    [SerializeField]
    private TextMeshProUGUI myPlayerIndicator;
    [SerializeField]
    private TurnTimer timerComponent;
    

    public static TurnManager INSTANCE {
        get
        {
            return instance;
        }

        set
        {
            // When setting to null, just set to null
            if(value == null)
            {
                instance = null;
                return;
            }

            // If we are trying to initialize the value of
            // the singleton, check whether it has already been initialed.
            Assertions.AssertNull(instance);

            instance = value;
        }
    }


    private void OnDestroy()
    {
        INSTANCE = null;
    }

    private void Awake()
    {
        Debug.Log("init");
        INSTANCE = this;
        
        if (!GameSetupValues.INSTANCE.IsOnline)
        {
            timerComponent.gameObject.SetActive(false);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateTurnComponent();
    }

    public static void UpdateTextComponentToMatchPlayer(TextMeshProUGUI text, Player player)
    {
        StaticPlayerInfoSO playerInfo = StaticPlayerInfo.INSTANCE.GetPlayerInfoByID(player);

        string playerRepresentation = playerInfo.StringPlayerRepresentation.ToUpper();
        Color playerColor = playerInfo.PlayerColor;

        text.text = playerRepresentation;

        text.color = playerColor;

    }

    public void UpdateTurnComponent()
    {

        UpdateTextComponentToMatchPlayer(turn, TurnPlayer);
        
        // todo: probably because this gets
        // called before the client connects - fix by calling some
        // method after the client connects.
        if(!NetPlayerManager.INSTANCE.IsClientConnected())
        {
            myPlayerImage.SetActive(false);
            myPlayerIndicator.gameObject.SetActive(false);
            return;
        }    
        UpdateTextComponentToMatchPlayer(myPlayerIndicator, NetPlayerManager.INSTANCE.GetLocalPlayer().GamePlayer);
    }

    /// <summary>
    /// The player who's turn it is.
    /// </summary>
    [SyncVar(hook = nameof(SyncTurnPlayer))]
    private Player turnPlayer;

    public Player TurnPlayer => turnPlayer;

    public void SyncTurnPlayer(Player oldValue, Player newValue)
    {
        turnPlayer = newValue;
        UpdateTurnComponent();
    }

    [Server]
    public void ServerSetTurnPlayer(Player newValue)
    {
        SyncTurnPlayer(turnPlayer, newValue);
    }

    [Server]
    public void ServerSetTurnPlayerToLoser()
    {
        if(GameSetupValues.INSTANCE.PlayingAgainstAi)
        {
            ServerSetTurnPlayer(Player.CIRCLE);
            return;
        }


        Player? winningPlayer = MatchInfo.INSTANCE.LastWinningPlayer;

        if(!winningPlayer.HasValue)
        {
            ServerSetTurnPlayerToRandom();
            return;
        }

        ServerSetTurnPlayer(winningPlayer.Value.GetNextCircular());
    }

    [Server]
    public void ServerSetTurnPlayerToRandom()
    {
        if (GameSetupValues.INSTANCE.PlayingAgainstAi)
        {
            ServerSetTurnPlayer(Player.CIRCLE);
            return;
        }

        System.Random rng = new System.Random();
        int playerSize = EnumUtil.GetEnumSize<Player>();
        int randomIndex = rng.Next(playerSize);
        Player pickedPlayer = EnumUtil.GetValueByIndex<Player>(randomIndex);

        Debug.Log("SERVER: Setiting host to " + pickedPlayer);
        NetPlayerManager.INSTANCE.HostPlayer.ServerSetGamePlayer(pickedPlayer);

        if(NetPlayerManager.INSTANCE.IsClientConnected())
        {
            Player nextPlayer = pickedPlayer.GetNextCircular();
            Debug.Log("SERVER: Setting client player to be " + nextPlayer);
            NetPlayerManager.INSTANCE.ClientPlayer.ServerSetGamePlayer(nextPlayer);
        }

        ServerSetTurnPlayer(pickedPlayer);

    }

    [Server]
    public void ServerNextTurn()
    {
        ServerShiftToNextTurnPlayer();
    }

    [Server]
    private void ServerShiftToNextTurnPlayer()
    {
        ServerSetTurnPlayer(turnPlayer.GetNextCircular());
    }

}
