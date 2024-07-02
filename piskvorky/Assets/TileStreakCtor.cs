using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileStreakCtor
{
    /// <summary>
    /// Creates a <see cref="TileStreak"/> out of a collection of <see cref="Vector2Int"/>s.
    /// !! BE AWARE THAT A LIMITATION EXISTS - YOU NEED TO SUPPLY THE TILE POSITIONS IN SUCH A WAY THAT NO TWO TILES ARE EVER
    /// MORE THAN 1 SPACE APART IN TILE COORDINATES !!
    /// SUPPLIED TILE POSITIONS ARE ALSO NOT CHECKED FOR VALIDITY
    /// </summary>
    public static TileStreak CreateTileStreakFromBatch(Player streakPlayer, StreakStatus streakStatus, Vector2Int[] tilePositions)
    {
        if(tilePositions.Length < 2)
        {
            Debug.LogError("Tried to create a streak batch from too few positions.");
            return null;
        }

        FieldTileData[] tiles = FieldTileDataCtor.ConstructTilesFromPositions(streakPlayer, tilePositions);
        Directions dir = DirectionsUtil.GetBasicDirectionFromTileToTile(tiles[0], tiles[1]);

        TileStreak tileStreak = new TileStreak(tiles[0], dir);
        tileStreak.SetStreakBlockage(streakStatus);

        for(int i = 1; i < tiles.Length; ++i)
        {
            FieldTileData tile = tiles[i];
            tileStreak.AddStreakTile(tile);
        }

        return tileStreak;
    }
}
