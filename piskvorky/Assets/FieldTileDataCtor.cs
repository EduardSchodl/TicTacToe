using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldTileDataCtor
{
    public static FieldTileData[] ConstructTilesFromPositions(Player? player, Vector2Int[] positions)
    {
        FieldTileData[] tiles = new FieldTileData[positions.Length];

        for(int i = 0; i < positions.Length; ++i)
        {
            Vector2Int position = positions[i];
            tiles[i] = new FieldTileData(position, player);
        }

        return tiles;
    }
}
