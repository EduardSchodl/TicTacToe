using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: STOP USING DIRECTIONS AND ADD A CLASS THAT HOLDS INFORMATION FOR EACH DIRECTION
static class DirectionsUtil
{
    private static Dictionary<Directions, string> directionsToPrefixStr = new Dictionary<Directions, string>();
    private static Dictionary<Directions, Vector2Int> directionFlagToChangeVector = new Dictionary<Directions, Vector2Int>();
    private static Dictionary<Directions, Directions> advancedDirectionsIntoBasicDirections = new Dictionary<Directions, Directions>();

    static DirectionsUtil()
    {
        directionsToPrefixStr.Add(Directions.NONE, "");
        directionsToPrefixStr.Add(Directions.NORTH, "N");
        directionsToPrefixStr.Add(Directions.EAST, "E");
        directionsToPrefixStr.Add(Directions.SOUTH, "S");
        directionsToPrefixStr.Add(Directions.WEST, "W");


        Vector2Int UP_VEC = new Vector2Int(0, 1);
        Vector2Int RIGHT_VEC = new Vector2Int(1, 0);

        directionFlagToChangeVector.Add(Directions.NONE, new Vector2Int(0, 0));
        directionFlagToChangeVector.Add(Directions.NORTH, UP_VEC);
        directionFlagToChangeVector.Add(Directions.EAST, RIGHT_VEC);
        directionFlagToChangeVector.Add(Directions.SOUTH, UP_VEC * -1);
        directionFlagToChangeVector.Add(Directions.WEST, RIGHT_VEC * -1);

        advancedDirectionsIntoBasicDirections.Add(Directions.SOUTH, Directions.NORTH);
        advancedDirectionsIntoBasicDirections.Add(Directions.SOUTH | Directions.WEST, Directions.NORTH | Directions.EAST);
        advancedDirectionsIntoBasicDirections.Add(Directions.WEST, Directions.EAST);
        advancedDirectionsIntoBasicDirections.Add(Directions.WEST | Directions.NORTH, Directions.SOUTH | Directions.EAST);
    }

    public static Directions GetDirectionFromTileToTile(FieldTileData t1, FieldTileData t2)
    {
        return GetDirectionFromVecToVec(t1.TilePosition, t2.TilePosition);
    }

    public static Directions GetBasicDirectionFromTileToTile(FieldTileData t1, FieldTileData t2)
    {
        return GetBasicDirectionFromVecToVec(t1.TilePosition, t2.TilePosition);
    }

    public static Directions GetBasicDirectionFromVecToVec(Vector2Int v1, Vector2Int v2)
    {
        Directions advancedDirections = GetDirectionFromVecToVec(v1, v2);
        return TurnAdvDirIntoBasic(advancedDirections);
    }

    public static Directions TurnAdvDirIntoBasic(Directions direction)
    {
        if(direction == Directions.NONE)
        {
            return Directions.NONE;
        }

        // Already basic direction
        if(!advancedDirectionsIntoBasicDirections.ContainsKey(direction))
        {
            return direction;
        }

        return advancedDirectionsIntoBasicDirections[direction];
    }

    public static Directions GetDirectionFromVecToVec(Vector2Int v1, Vector2Int v2)
    {
        Vector2Int diff = v2 - v1;

        if (diff.AnyAbsCoordinateIsLargerThan(1))
        {
            Debug.LogError($"Cannot create direction for two vectors which are more than 1 space apart {v1} {v2}.");
            return Directions.NONE;
        }

        return GetVectorAsDirection(diff);
    }


    public static Directions GetVectorAsDirection(Vector2Int vector)
    {
        if (vector.AnyAbsCoordinateIsLargerThan(1))
        {
            Debug.LogError($"Cannot create direction for {vector} which has coordinates larger than 1.");
            return Directions.NONE;
        }

        var brokenVector = vector.BreakVectorCoordinates();
        Directions direction = ReverseVectorIntoDirectionFlag(brokenVector.xVec) | ReverseVectorIntoDirectionFlag(brokenVector.yVec);
        return direction;
    }

    private static Directions ReverseVectorIntoDirectionFlag(Vector2Int basicVector)
    {
        foreach(KeyValuePair<Directions, Vector2Int> valuePair in directionFlagToChangeVector)
        {
            Directions dirFlag = valuePair.Key;
            Vector2Int dirVector = valuePair.Value;

            if(basicVector == dirVector)
            {
                return dirFlag;
            }
        }

        Debug.LogError($"{basicVector} is not a key of {nameof(directionFlagToChangeVector)}.");
        return Directions.NONE;
    }

    public static Vector2Int GetDirectionAsVector(Directions direction)
    {
        Vector2Int resultingVector = Vector2Int.zero;
        foreach(Directions flag in direction.IterateFlags())
        {
            resultingVector += directionFlagToChangeVector[flag];
        }

        return resultingVector;
    }
    
    /// <summary>
    /// Iterates the directions clockwise going from NORTH to SOUTH EAST.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Directions> IterateDirectionsBasicClockwise()
    {
        yield return Directions.NORTH;
        yield return Directions.NORTH | Directions.EAST;
        yield return Directions.EAST;
        yield return Directions.SOUTH | Directions.EAST;
    }

    public static IEnumerable<Directions> IterateDirectionsClockwise()
    {
        foreach(Directions basicDir in IterateDirectionsBasicClockwise())
        {
            yield return basicDir;
        }

        yield return Directions.SOUTH;
        yield return Directions.SOUTH | Directions.WEST;
        yield return Directions.WEST;
        yield return Directions.NORTH | Directions.WEST;
    }

    public static IEnumerable<Vector2Int> IterateDirectionVectorsBasicClockwise()
    {
        foreach (Directions dir in IterateDirectionsBasicClockwise())
        {
            yield return GetDirectionAsVector(dir);
        }
    }

    public static IEnumerable<Vector2Int> IterateDirectionVectorsClockwise()
    {
        foreach(Directions dir in IterateDirectionsClockwise())
        {
            yield return GetDirectionAsVector(dir);
        }
    }

    public static string GetDirectionPrefix(Directions direction)
    {
        string result = "";
        Directions[] directionValues = (Directions[]) Enum.GetValues(typeof(Directions));
        

        for(int i = 0; i < directionValues.Length; ++i)
        {
            Directions dir = directionValues[i];
            if(!direction.HasFlag(dir))
            {
                continue;
            }
            
            result += directionsToPrefixStr[dir];
        }

        return result;
    }
}