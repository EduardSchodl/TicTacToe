using UnityEngine;

public struct TileManipulationStackEntry
{

    private Vector2Int manipulatedTile;
    public Vector2Int ManipulatedTile => manipulatedTile;

    private Player? tileSetValue;
    public Player? TileSetValue => tileSetValue;

    public TileManipulationStackEntry(Vector2Int manipulatedTile, Player? tileSetValue)
    {
        this.manipulatedTile = manipulatedTile;
        this.tileSetValue = tileSetValue;
    }

}