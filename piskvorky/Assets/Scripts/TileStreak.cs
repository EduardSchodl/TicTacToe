using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStreak
{
    /// <summary>
    /// Returns if this <see cref="TileStreak"></see> would end the game immediately.
    /// </summary>
    public bool ConstitutesImmediateWin()
    {
        return StreakTileCount >= GameSetupValues.INSTANCE.PiecesNeededToWin;
    }

    /// <summary>
    /// Returns if this <see cref="TileStreak"></see> guarantees a win for its owner, i.e.
    /// it is impossible for the player or ai to block this.
    /// </summary>
    /// <returns></returns>
    public bool ConstitutesGuaranteedWin()
    {
        return StreakTileCount >= (GameSetupValues.INSTANCE.PiecesNeededToWin - 1) && TileStreakStatus == StreakStatus.FREE;
    }

    public TileStreak(FieldTileData startingTile, Directions streakMainDirection)
    {
        Player? tilePlayer = startingTile.TilePlayer;
        if (!tilePlayer.HasValue)
        {
            Debug.LogError($"Tried to create a {nameof(TileStreak)} from a tile with no player.");
        }

        StreakOwner = startingTile.TilePlayer.Value;
        StreakMainDirection = streakMainDirection;
        
        AddStreakTile(startingTile);
    }

    /// <summary>
    /// Use this in testing only!
    /// StreakStatus is computed rather than set by the user.
    /// </summary>
    public void SetStreakBlockage(StreakStatus streakStatus)
    {
        TileStreakStatus = streakStatus;
    }

    public bool ContainsStreakTile(FieldTileData tile)
    {
        return StreakTiles.Contains(tile);
    }

    public IEnumerable<FieldTileData> IterateStreakTiles()
    {
        foreach(FieldTileData streakTile in StreakTiles)
        {
            yield return streakTile;
        }
    }

    public void AddStreakTile(FieldTileData tile)
    {
        StreakTiles.AddLast(tile);
    }

    public int StreakTileCount => StreakTiles.Count;

    private LinkedList<FieldTileData> StreakTiles
    {
        get;
        set;
    } = new LinkedList<FieldTileData>();

    public Player StreakOwner
    {
        get;
        private set;
    }

    public Directions StreakMainDirection
    {
        get;
        set;
    }

    public StreakStatus TileStreakStatus
    {
        get;
        private set;
    } = StreakStatus.FREE;

    public void IncreaseStreakBlockage()
    {
        if(TileStreakStatus == StreakStatus.BLOCKED)
        {
            Debug.LogError($"Tried to increase streak blockage past BLOCKED.");
            return;
        }

        TileStreakStatus = TileStreakStatus.GetNextCircular();
    }

    public override bool Equals(object obj)
    {
        TileStreak objStreak = obj as TileStreak;

        if(objStreak == null)
        {
            return false;
        }

        if(this == objStreak)
        {
            return true;
        }

        bool sameOwner = StreakOwner == objStreak.StreakOwner;
        bool sameTileCount = StreakTileCount == objStreak.StreakTileCount;

        if (!sameOwner || !sameTileCount)
        {
            return false;
        }

        bool sameStatus = TileStreakStatus == objStreak.TileStreakStatus;
        bool sameDirection = StreakMainDirection == objStreak.StreakMainDirection;

        if(!sameStatus || !sameDirection)
        {
            return false;
        }

        foreach(FieldTileData objTile in objStreak.StreakTiles)
        {
            if(!StreakTiles.Contains(objTile))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        hashCode += (int) StreakOwner;
        hashCode += StreakTileCount;
        hashCode += (int) StreakMainDirection;
        hashCode += (int) TileStreakStatus;

        foreach (FieldTileData objTile in StreakTiles)
        {
            hashCode += objTile.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString()
    {
        string str = $"{nameof(TileStreak)}\n";
        str += $"Owner: {StreakOwner}\n";
        str += $"TileCount: {StreakTileCount}\n";
        str += $"Tiles: [";

        foreach(FieldTileData tile in StreakTiles)
        {
            bool isLast = StreakTiles.Last.Value == tile;
            string postfix = isLast ? "" : ", ";
            str += $"{tile}{postfix}";
        }

        str += $"]\n";
        str += $"Status: {TileStreakStatus}\n";
        str += $"Direction: {StreakMainDirection}\n";
        return str;
    }
}
