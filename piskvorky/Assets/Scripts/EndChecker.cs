using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndChecker
{
    public static (bool gameEnded, Player? winningPlayer) CheckGameEnded(Field field)
    {
        // First check whether anyone has won
        // Take all filled tiles
        foreach (KeyValuePair<Vector2Int, FieldTileData> filledTile in field.IterateFilledTiles())
        {
            StreakCollection streaks = ConstructAllStreaksFromTile(field, filledTile.Value);
            bool gameEnded = streaks.AnyStreakConstitutesWin();

            if (gameEnded)
            {
                return (true, filledTile.Value.TilePlayer);
            }

        }

        // Second check for draws - No tiles left to place constitues a draw, thus the game ended.
        if (field.HasAllTilesFilled())
        {
            // null = draw => no player won or lost
            return (true, null);
        }

        // null = no player has won yet, game has not yet ended
        return (false, null);
    }


    /// <summary>
    /// A faster version of <see cref="CheckGameEnded(Field)"/> which does
    /// not scan the whole field, but rather only checks whether a player has won from a single tile
    /// The method also checks for draws. This is useful as you need to check
    /// only a single tile each time a marker is placed during the game, not the whole field.
    /// This speeds up processing time.
    /// 
    /// The method returns whether the game has ended, this means either a win or a draw.
    /// This will return true even if the player of the <see cref="FieldTileData"/> passed in
    /// the parameter has not won (draw state). If you want to check for wins only use <see cref="ConstructAllStreaksFromTile(Field, FieldTileData)"/>
    /// </summary>
    public static (bool gameEnded, Player? winningPlayer, TileStreak winningStreak) CheckGameEndedFromTile(Field field, FieldTileData tileData)
    {
        
        StreakCollection streaks = ConstructAllStreaksFromTile(field, tileData);

        TileStreak winningStreak = streaks.TryGetWinningStreak();
        bool hasWin = winningStreak != null;

        // Won
        if(hasWin)
        {
            return (hasWin, tileData.TilePlayer, winningStreak);
        }

        // Draw = game has ended
        if (field.HasAllTilesFilled())
        {
            return (true, null, null);
        }


        return (false, null, null);


    }

    public static StreakCollection ConstructAllStreaksFromTile(Field field, FieldTileData tileData)
    {
        return ConstructAllStreaksFromTile(null, field, tileData);
    }

    /// <summary>
    /// Returns whether the game has been won from a tile.
    /// This does not check whether the game is a draw. The method checks only for
    /// wins from that tile and nothing else.
    /// 
    /// To include a check for draws use: <see cref="CheckGameEndedFromTile(Field, FieldTileData)(Field)"/>
    /// To scan the whole <see cref="Field"/> use <see cref="CheckGameEnded(Field)"/>
    /// </summary>
    public static StreakCollection ConstructAllStreaksFromTile(StreakCollection streakCollection, Field field, FieldTileData tileData)
    {
        StreakCollection streaks = new StreakCollection();

        // Streaks cannot be constructed from non-marked tiles
        if (!tileData.IsOccupied())
        {
            Debug.LogError("Tried to construct streak from an empty tile.");
            return null;
        }

        // Odd scenario
        if(!field.ContainsFieldTileData(tileData))
        {
            Debug.LogError("Tried to construct a streak from a tile which is not part of the field passed as the parameter.");
            return null;
        }

        Vector2Int positionToCheckFrom = tileData.TilePosition;
        
        // Try to look in all the main directions and their opposites
        foreach (Directions mainDirection in DirectionsUtil.IterateDirectionsBasicClockwise())
        {
            if(streakCollection != null && streakCollection.ContainsStreakForTileInCollection(tileData, mainDirection))
            {
                continue;
            }

            // Main direction turned into a direction vector
            Vector2Int changeVec = DirectionsUtil.GetDirectionAsVector(mainDirection);

            TileStreak streak = new TileStreak(tileData, mainDirection);

            int tracker = 0;
            int trackerReverse = 0;

            // Look for tiles in the main direction
            //     [x]
            //    x
            //  x
            while (CheckNextTileForStreak(field, ref streak, positionToCheckFrom, changeVec, ref tracker)) ;

            // And its opposite
            //      x
            //    x
            // [x]
            while (CheckNextTileForStreak(field, ref streak, positionToCheckFrom, changeVec * -1, ref trackerReverse)) ;

            // Ignore lone tiles
            // Single tiles do not constitute a TileStreak
            if(streak.StreakTileCount == 1)
            {
                continue;
            }
            streaks.AddStreak(streak);
        }

        return streaks;
    }


    private static bool CheckNextTileForStreak(Field field, ref TileStreak tileStreak, Vector2Int checkingFromPos, Vector2Int changeVector, ref int tileTracker)
    {
        // We're checking the fifth tile, this is the break condition as we should
        // not check further.
        if (tileTracker == GameSetupValues.INSTANCE.PiecesNeededToWin)
        {
            return false;
        }

        // The difference vector to be added to the position we are checking from
        // in order to obtain the final position of the tile that will be checked.
        Vector2Int changeVecMultipl = (changeVector * (tileTracker + 1));
        // The field position of the tile that will be checked by this method call.
        Vector2Int checkedTilePos = checkingFromPos + changeVecMultipl;

        bool tileExists = field.TileExists(checkedTilePos);
        
        // If we get out of bounds, we cannot continue further
        if (!tileExists)
        {
            // Out of bounds does also constitue streak blockage
            tileStreak.IncreaseStreakBlockage();
            return false;
        }

        FieldTileData checkedTileData = field.GetTile(checkedTilePos);
        
        // Unoccupied tiles cannot contain our player.
        if (!checkedTileData.IsOccupied())
        {
            return false;
        }

        // Incorrect player found
        if (checkedTileData.GetTilePlayer() != tileStreak.StreakOwner)
        {
            // Streak blocked by another player
            tileStreak.IncreaseStreakBlockage();
            return false;
        }


        // All checks succeeded
        tileStreak.AddStreakTile(checkedTileData);

        // Move onto the next tile
        ++tileTracker;
        return true;
    }

    /// <summary>
    /// Collects all the streaks on the given <see cref="Field"/>.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static StreakCollection CollectAllStreaks(Field field)
    {
        StreakCollection fieldStreaks = new StreakCollection();

        foreach(KeyValuePair<Vector2Int, FieldTileData> tile in field.IterateFilledTiles())
        {
            StreakCollection createdStreaks = ConstructAllStreaksFromTile(fieldStreaks, field, tile.Value);
            fieldStreaks.AddStreakCollection(createdStreaks);
        }

        return fieldStreaks;
    }


}
