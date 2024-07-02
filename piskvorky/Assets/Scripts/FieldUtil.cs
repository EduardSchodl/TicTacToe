using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldUtil
{

    public static IEnumerable<Vector2Int> GetTilesAroundTileTopLeft(Vector2Int centerTile, int radius, int fieldMaxX, int fieldMaxY)
    {
        if(radius == 0)
        {
            Debug.LogWarning($"{nameof(GetTilesAroundTileTopLeft)} was called with {nameof(radius)} = 0." +
                $"This is a situation, while not incorrect, should not arise as of now.");
            yield break;
        }
        // centerTile = (1, 1)
        // radius = 1
        // r = 1 equates to 8 tiles (3*3 - 1)
        // r = 2 equates to 15 tiles
        // size
        // (r + 2)^2 - 1

        // r = 1
        // this makes a 3x3 square without the center
        // side len = 3

        // for radius 1 = 3x3 square, sideLength = 3
        int sideLength = radius + 2;

        // maxXY offset is for 3x3 = 2
        int maxXYOffset = sideLength - 2;

        // with each 1 radius, to get to the top left tile
        // we go 1 more y up
        // and 1 more x left
        
        int startY = MathUtil.IntClamp(centerTile.y + maxXYOffset, 0, fieldMaxY);
        int endY = MathUtil.IntClamp(centerTile.y - maxXYOffset, 0, fieldMaxY);

        int startX = MathUtil.IntClamp(centerTile.x - maxXYOffset, 0, fieldMaxX);
        int endX = MathUtil.IntClamp(centerTile.x + maxXYOffset, 0, fieldMaxX);

        for(int y = startY; y >= endY; --y)
        {
            for(int x = startX; x <= endX; ++x)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if(pos == centerTile)
                {
                    continue;
                }

                yield return pos;
            }
        }

    }

    public static List<Vector2Int> CollectTilesAroundTileTopLeft(Vector2Int centerTile, int radius, int fieldWidth, int fieldHeight)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();

        foreach(Vector2Int tile in GetTilesAroundTileTopLeft(centerTile, radius, fieldWidth, fieldHeight))
        {
            tiles.Add(tile);
        }

        return tiles;
    }

}
