using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTileData
{
    public Vector2Int TilePosition
    {
        get;
        set;
    }

    public Player? TilePlayer
    {
        get;
        set;
    }

    public FieldTile TileScript
    {
        get;
        set;
    }

    public FieldTileData(FieldTileData copyObject): this(copyObject.TilePosition, copyObject.TilePlayer, copyObject.TileScript)
    {

    }

    public FieldTileData(Vector2Int tilePosition, Player? tilePlayer, FieldTile tileScript): this(tilePosition, tilePlayer)
    {
        TileScript = tileScript;
    }

    public FieldTileData(Vector2Int tilePosition, Player? tilePlayer)
    {
        TilePosition = tilePosition;
        TilePlayer = tilePlayer;
    }

    public FieldTileData()
    {

    }

    public Player GetTilePlayer()
    {
        if(!IsOccupied())
        {
            throw new System.Exception("Tried to get a tile player where there is none.");
        }
        return TilePlayer.Value;
    }

    public bool IsOccupied()
    {
        return TilePlayer.HasValue;
    }

    public bool DataEquals(object obj)
    {
        FieldTileData objData = obj as FieldTileData;
        if (objData == null)
        {
            return false;
        }

        if(this == objData)
        {
            return true;
        }

        bool samePosition = TilePosition == objData.TilePosition;
        bool samePlayer = TilePlayer == objData.TilePlayer;
        return samePosition && samePlayer;
    }

    public override bool Equals(object obj)
    {
        return DataEquals(obj);
    }

    public override int GetHashCode()
    {
        return TilePosition.x + (-1 * TilePosition.y) + (TilePlayer.HasValue ? (int)TilePlayer.Value : 0);
    }

    public override string ToString()
    {
        string tileScriptStr = TileScript != null ? $"{TileScript}" : "null";
        return $"(TilePosition: {TilePosition}, Player: {TilePlayer}, TileScript: {tileScriptStr})";
    }
}
