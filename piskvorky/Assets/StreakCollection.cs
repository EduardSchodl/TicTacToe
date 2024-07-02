using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains a collection of <see cref="TileStreak"/>s.
/// </summary>
public class StreakCollection
{
    private List<TileStreak> Streaks
    {
        get;
        set;
    } = new List<TileStreak>();

    /// <summary>
    /// Used in testing only.
    /// </summary>
    public List<TileStreak> GetStreaks()
    {
        return Streaks;
    }

    public IEnumerable<TileStreak> IterateStreaks()
    {
        foreach (TileStreak streak in Streaks)
        {
            yield return streak;
        }
    }

    public int StreakCount => Streaks.Count;

    public bool ContainsStreakForTileInCollection(FieldTileData tile, Directions mainDirection)
    {
        foreach(TileStreak streak in Streaks)
        {
            if(streak.StreakMainDirection != mainDirection)
            {
                continue;
            }

            if(!streak.ContainsStreakTile(tile))
            {
                continue;
            }

            return true;
        }

        return false;
    }

    public override string ToString()
    {
        string str = $"{nameof(StreakCollection)}:\n";
        for(int i = 0; i < StreakCount; ++i)
        {
            string newLine = (i != StreakCount - 1) ? "\n" : "";
            str += $"{i}: {Streaks[i]}{newLine}";
        }
        return str;
    }

    private void AddStreaks(List<TileStreak> streaks)
    {
        Streaks.AddRange(streaks);
    }

    public void AddStreakCollection(StreakCollection streakCollection)
    {
        AddStreaks(streakCollection.Streaks);
    }

    public void AddStreak(TileStreak tileStreak)
    {
        Streaks.Add(tileStreak);
    }

    public TileStreak TryGetWinningStreak()
    {
        foreach (TileStreak streak in Streaks)
        {
            if (streak.ConstitutesImmediateWin())
            {
                return streak;
            }
        }

        return null;
    }

    public bool AnyStreakConstitutesWin()
    {
        return TryGetWinningStreak() != null;
    }

}
