using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

[RequireComponent(typeof(GameSetupSettingValues))]
public class PlayerScript : NetworkBehaviour
{
    private GameSetupSettingValues gameSetupVals;

    [SyncVar(hook = nameof(SyncSize))]
    public Vector2Int fields = new Vector2Int();

    [SyncVar(hook = nameof(SyncPiecesNeededToWin))]
    public int piecesNeededToWin = 0;

    [SyncVar(hook = nameof(SyncWinsNeededForMatchWin))]
    public int winsNeededForMatchWin = 0;

    [SerializeField]
    private GameObject playingField;

    [SerializeField]
    private GameObject fieldValue;

    [SerializeField]
    private GameObject piecesValue;

    [SerializeField]
    private GameObject numOfMatches;

    void Start()
    {
        gameSetupVals = GetComponent<GameSetupSettingValues>();
        Debug.Log(gameSetupVals);

        if(isServer)
        {
            fields = GameSetupValues.INSTANCE.FieldSize;
            piecesNeededToWin = GameSetupValues.INSTANCE.PiecesNeededToWin;
            winsNeededForMatchWin = GameSetupValues.INSTANCE.TotalRoundsPlayed;
        }
        
        if (!isServer && isLocalPlayer)
        {
            //Destroy(GameObject.Find("PlayingField"));
            //SetField();
        }

        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Change();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }
    }

    public void OnConnectedToServer()
    {
        fieldValue.GetComponent<TextMeshProUGUI>().text = fields.ToString();
        piecesValue.GetComponent<TextMeshProUGUI>().text = piecesNeededToWin.ToString();
        numOfMatches.GetComponent<TextMeshProUGUI>().text = winsNeededForMatchWin.ToString();
    }

    public void SetValues()
    {
        
    }

    //Metoda se zavolá døíve, než se stihnou nastavit hodnoty ze sítì
    public void SetField()
    {
        // 5.9.2022 - 18:22 jsem ti to odmazal - Pavel
    }

    private void SyncSize(Vector2Int oldX, Vector2Int newX)
    {
        Debug.Log("sync called");
        this.fields = newX;
    }

    private void SyncPiecesNeededToWin(int oldX, int newX)
    {
        this.piecesNeededToWin = newX;
    }

    private void SyncWinsNeededForMatchWin(int oldX, int newX)
    {
        this.winsNeededForMatchWin = newX;
    }

    //For testing
    public void Change()
    {
        fields = new Vector2Int(2, 1);
    }
}
