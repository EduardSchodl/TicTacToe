using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerInfo : MonoBehaviour
{
    public const string FIELD_SEPARATOR_CHARACTER = "|";
    public const string EMPTY_TILE_INDICATOR = " ";

    private static StaticPlayerInfo instance;

    public static StaticPlayerInfo INSTANCE
    {
        get
        {
            return instance;
        }

        set
        {
            Assertions.AssertNull(instance);
            instance = value;
        }
    }

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        INSTANCE = this;
    }

    [SerializeField]
    private StaticPlayerInfoSO[] registeredPlayers;

    public Player GetPlayerByMark(string mark)
    {
        return Array.Find(registeredPlayers, (regPlayer) => regPlayer.StringPlayerRepresentation == mark).PlayerID;
    }

    public StaticPlayerInfoSO GetPlayerInfoByID(Player player)
    {
        return Array.Find(registeredPlayers, (regPlayer) => regPlayer.PlayerID == player);
    }
}
