using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEvaluator
{
    /// <summary>
    /// Evalutes value of <see cref="Field"/> passed
    /// in the parameter as if we were observing the field from the observer <see cref="Player"/>'s
    /// perspective.
    /// 
    /// It returns an <see cref="int"/> value representing how
    /// good/valuable is this field state for the observing player.
    /// 
    /// Say for example if we are one tile away from a win, then this
    /// <see cref="Field"/> would be very good for us and thus
    /// a large value would be returned.
    /// </summary>
    public static int EvaluateFieldValue(Field field, Player observerPlayer)
    {
        // Take all the streaks in the game
        StreakCollection allStreaks = EndChecker.CollectAllStreaks(field);

        int fieldValue = 0;

        foreach(TileStreak streak in allStreaks.IterateStreaks())
        {
            // Compute the value of each streak
            int computedValue = EvaluateStreakValue(streak);
            
            // Is this streak ours?
            bool isComputingForUs = streak.StreakOwner == observerPlayer;

            // If we are computing for the player, doubly his points
            // to try to encourage the ai to make more defensive moves than offensive
            // ones.
            computedValue *= isComputingForUs ? 1 : 2;

            // Either add the value of the streak if we are computing
            // for the observer or subtract it if we are computing
            // for anyone else (good streaks add value, bad streaks subtract value).
            fieldValue += (isComputingForUs ? 1 : -1) * computedValue;
        }

        return fieldValue;
    }

    public const int WIN_VALUE = 100_000_000;
    public const int WINNING_STREAK_VALUE = 90_000_000;
    public const int SINGLE_TILE_VALUE = 10;

    public static int EvaluateStreakValue(TileStreak tileStreak)
    {
        // Place mark that wins
        if(tileStreak.ConstitutesImmediateWin())
        {
            return WIN_VALUE;
        }

        // Place mark that makes guaranteed win streak (even if opponent blocks this streak - placing a marker that wins
        // the game takes over on the next turn)
        if (tileStreak.ConstitutesGuaranteedWin())
        {
            return WINNING_STREAK_VALUE;
        }

        int tileValues = tileStreak.StreakTileCount * SINGLE_TILE_VALUE;
        int blockageCount = tileStreak.TileStreakStatus.GetBlockageCount();
        return tileValues * (2 - blockageCount);
    }
}
