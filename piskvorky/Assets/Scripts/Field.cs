using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field
{

    public Field()
    {

    }

    public Field(string fieldString)
    {
        int inputLength = fieldString.Length;
        
        if (inputLength == 0)
        {
            Debug.LogError("Tried to create a field out of an empty string.");
            return;
        }

        var testResult = MathUtil.FieldStrMakesSquare(inputLength);
        if (!testResult.makesSquare)
        {
            Debug.LogWarning($"{inputLength} does not constitute a square.");
            return;
        }

        StaticPlayerInfo staticPlayerInfo = StaticPlayerInfo.INSTANCE;
        Vector2Int[] squarePositions = VectorUtil.GetSquarePositionsTopLeft(testResult.squareLength);

        int positionIndex = 0;

        for (int i = 0; i < fieldString.Length; ++i)
        {
            char character = fieldString[i];
            string str = character.ToString();

            if(str == StaticPlayerInfo.FIELD_SEPARATOR_CHARACTER)
            {
                continue;
            }

            Player? player = str == StaticPlayerInfo.EMPTY_TILE_INDICATOR ? null : staticPlayerInfo.GetPlayerByMark(str);
            
            Vector2Int position = squarePositions[positionIndex];

            FieldTileData tileData = new FieldTileData(position, player);
            AddTileByData(position, tileData);

            ++positionIndex;
        }
    }

    /// <summary>
    /// The Dictionary holding <see cref="FieldTile"/> scripts by their position.
    /// </summary>
    public Dictionary<Vector2Int, FieldTileData> Tiles
    {
        get;
        private set;
    } = new Dictionary<Vector2Int, FieldTileData>();

    public readonly Stack<TileManipulationStackEntry> TileManipulationStack = new Stack<TileManipulationStackEntry>();

    public int GetTilesCount()
    {
        return Tiles.Count;
    }

    public FieldTileData TryGetTile(Vector2Int tileCoords)
    {
        Tiles.TryGetValue(tileCoords, out FieldTileData fetchedTile);
        return fetchedTile;
    }

    public FieldTileData GetTile(Vector2Int tileCoords)
    {
        return Tiles[tileCoords];
    }

    public KeyValuePair<Vector2Int, FieldTileData> GetTileAsKeyValuePair(Vector2Int tileCoords)
    {
        return new KeyValuePair<Vector2Int, FieldTileData>(tileCoords, GetTile(tileCoords));
    }

    public void TurnOffLastTileHalo()
    {
        // No tile to remove halo off from.
        if(TileManipulationStack.Count == 0)
        {
            return;
        }

        TileManipulationStackEntry lastEntry = TileManipulationStack.Peek();
        GetTile(lastEntry.ManipulatedTile).TileScript.TurnOffTileHalo();
    }

    /// <summary>
    /// Sets the player for a given tile without performing any check
    /// whether the tile is occupied.
    /// </summary>
    public void SetPlayerForTile(Vector2Int tileCoords, Player? player, bool updateScriptSprite = false)
    {
        FieldTileData fieldTileData = GetTile(tileCoords);
        fieldTileData.TilePlayer = player;

        if(updateScriptSprite)
        {
            FieldTile tileScript = fieldTileData.TileScript;
            tileScript.SetIndicatorToPlayer(player);
            tileScript.TurnOnTileHalo();

            TurnOffLastTileHalo();

        }

        TileManipulationStack.Push(new TileManipulationStackEntry(tileCoords, player));
    }

    public IEnumerable<TileManipulationStackEntry> IterateLastManipulatedTiles(int count)
    {
        return TileManipulationStack.Take(count);
    }

    public FieldTileData GetTile(int x, int y)
    {
        return GetTile(new Vector2Int(x, y));
    }

    public bool TileExists(Vector2Int tilePosition)
    {
        return Tiles.ContainsKey(tilePosition);
    }

    /// <summary>
    /// Adds an entry into the <see cref="Tiles"/> dictionary. This does not result
    /// in any in-game side effects such as game object instantiation.
    /// </summary>
    public void AddTile(Vector2Int tileCoords, FieldTile tile)
    {
        // Only one tile per coordinate
        if(TileExists(tileCoords))
        {
            Debug.LogError($"Tried to add a tile on an already existing position ({tileCoords})");
            return;
        }

        FieldTileData data = new FieldTileData();
        data.TilePosition = tileCoords;
        data.TileScript = tile;
        AddTileByData(tileCoords, data);
    }

    public void AddTileByData(Vector2Int tileCoords, FieldTileData tileData)
    {
        Tiles.Add(tileCoords, tileData);
    }

    /// <summary>
    /// <inheritdoc cref="AddTile(Vector2Int, FieldTile)"/>
    /// </summary>
    public void AddTile(int x, int y, FieldTile tile)
    {
        AddTile(new Vector2Int(x, y), tile);
    }

    /// <summary>
    /// Returns a tile script (<see cref="FieldTile"/>) by its coordinates on the playing field.
    /// </summary>
    public FieldTile GetTileScript(Vector2Int tileCoords)
    {
        if (!Tiles.ContainsKey(tileCoords))
        {
            return null;
        }

        return GetTile(tileCoords).TileScript;
    }

    /// <summary>
    /// <inheritdoc cref="GetTileScript(Vector2Int)"/>
    /// </summary>
    public FieldTile GetTileScript(int x, int y)
    {
        return GetTileScript(new Vector2Int(x, y));
    }

    /// <summary>
    /// Returns whether a <see cref="GameObject"/> passed as the parameter represents
    /// one of the <see cref="FieldTile"/>s of THIS <see cref="PlayingField"/>.
    /// </summary>
    public bool IsFieldsTile(GameObject gameObject)
    {
        if(!gameObject)
        {
            return false;
        }

        FieldTile tile = gameObject.GetComponent<FieldTile>();

        // Objects without the FieldTile script cannot be fields in a playing field
        if (!tile)
        {
            return false;
        }

        return ContainsFieldTileScript(tile);
    }

    public bool ContainsFieldTileData(FieldTileData fieldTileData)
    {
        return Tiles.Values.Contains(fieldTileData);
    }

    public bool ContainsFieldTileScript(FieldTile tile)
    {
        return GetFieldTileDataByScript(tile) != null;
    }

    public FieldTileData GetFieldTileDataByScript(FieldTile script)
    {
        foreach (FieldTileData fieldTileData in Tiles.Values)
        {
            if (script == fieldTileData.TileScript)
            {
                return fieldTileData;
            }
        }

        return null;
    }

    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTiles()
    {
        return IterateTiles(false);
    }

    /*
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundFilledTiles()
    {
        foreach(KeyValuePair<Vector2Int, FieldTileData> tile in EmptyTilesAroundFilledTiles)
        {
            yield return tile;
        }
    }
    */

    public KeyValuePair<Vector2Int, FieldTileData>[] CollectTilesAroundFilledTiles()
    {
        return Enumerable.ToArray(IterateEmptyTilesAroundFilledTiles(1));
    }


    /// <summary>
    /// Iterates all the empty tile within a given radius that are around
    /// already filled tiles.
    /// </summary>
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundFilledTiles(int radius)
    {
       foreach(KeyValuePair<Vector2Int, FieldTileData> tile in IterateEmptyTilesAroundTileCollection(IterateFilledTiles(), radius))
       {
            yield return tile;
       }
    }
    
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateLastManipulatedTilesAsKeyValuePair(int count)
    {
        foreach(TileManipulationStackEntry stackEntry in IterateLastManipulatedTiles(count))
        {
            yield return new KeyValuePair<Vector2Int, FieldTileData>(stackEntry.ManipulatedTile, GetTile(stackEntry.ManipulatedTile));
        }
    }

    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundLastManipulatedTiles(int lastXTiles, int radius)
    {
        foreach (KeyValuePair<Vector2Int, FieldTileData> tile in
            IterateEmptyTilesAroundTileCollection(
                IterateLastManipulatedTilesAsKeyValuePair(lastXTiles),
                radius
                )
            )
        {
            yield return tile;
        }
    }

    public IEnumerable<Vector2Int> IterateEmptyTilesAroundLastManipulatedTilesCollectPositions(int lastXTiles, int radius)
    {
        foreach(KeyValuePair<Vector2Int, FieldTileData> pair in IterateEmptyTilesAroundLastManipulatedTiles(lastXTiles, radius))
        {
            yield return pair.Key;
        }
    }


    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundTileCollection(IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> tileCollection, int radius)
    {
        // Two tiles in an opened field (with a fetch radius of 1) next to each other, each will fetch
        // 7 empty tiles around themselves, out of which 4 would have already been processed
        // by the previous tile. Therefore each tile should skip tiles which have already been
        // processed.
        HashSet<Vector2Int> processedTiles = new HashSet<Vector2Int>();

        // Start processing filled tiles from which we will get the empty tiles.
        foreach (KeyValuePair<Vector2Int, FieldTileData> filledTile in tileCollection)
        {
            // Take all the empty tiles which have not yet been processed
            // around the filled tile.
            foreach (var emptyTile in IterateEmptyTilesAroundTile(filledTile, processedTiles, radius))
            {
                yield return emptyTile;
            }
        }

    }

    /// <summary>
    /// Iterates the empty tiles around a <see cref="Vector2Int"/> tile as specified
    /// by the parameter in this <see cref="Field"/>.
    /// 
    /// There will be no tiles fetched that exceed the dimensions of this <see cref="Field"/>.
    /// The tiles will be fetched in a radius as specified by the parameter.
    /// </summary>
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateEmptyTilesAroundTile(
        KeyValuePair<Vector2Int, FieldTileData> tile,
        HashSet<Vector2Int> processedTiles,
        int radius
        )
    {

        // Get the dimensions of the field so we don't
        // try to create tiles that are out of bounds.
        // TODO: THIS SHOULD BE OPTIMIZED - CACHED
        var dimensions = GetFieldMaxValues();
        int fieldWidth = dimensions.width;
        int fieldHeight = dimensions.height;


        // Get the tile's position
        Vector2Int tilePosition = tile.Key;

        // Iterate ALL tiles [even filled ones] surrounding this tile
        // (within a given radius and ignoring tiles which are out of bounds)
        foreach (Vector2Int surroundingTilePos in FieldUtil.GetTilesAroundTileTopLeft(tilePosition, radius, fieldWidth, fieldHeight))
        {
            // Skip already processed tiles which have been fetched
            // by other tiles.
            if (processedTiles.Contains(surroundingTilePos))
            {
                continue;
            }

            // New tile (empty or filled) which has not yet been processed.
            FieldTileData emptyTile = GetTile(surroundingTilePos);

            // Ignore occupied tiles, we want only empty ones
            if (emptyTile.IsOccupied())
            {
                continue;
            }

            // Process the empty tile
            yield return new KeyValuePair<Vector2Int, FieldTileData>(surroundingTilePos, emptyTile);

            // Add the tile to the list of processed tiles
            processedTiles.Add(surroundingTilePos);
        }
    }

    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateFilledTiles()
    {
        return IterateTiles(true);
    }

    public bool HasAllTilesFilled()
    {
        int filledTileCount = 0;
        foreach(KeyValuePair<Vector2Int, FieldTileData> filledTile in IterateFilledTiles())
        {
            ++filledTileCount;
        }
        return filledTileCount == GetTilesCount();
    }

    /// <summary>
    /// == NOT TESTED ==
    /// </summary>
    public IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateFilledTilesByPlayer(Player player)
    {
        foreach(KeyValuePair<Vector2Int, FieldTileData> tile in IterateFilledTiles())
        {
            if(tile.Value.TilePlayer != player)
            {
                continue;
            }

            yield return tile;
        }

        yield break;
    }

    private IEnumerable<KeyValuePair<Vector2Int, FieldTileData>> IterateTiles(bool iterateFilledTiles)
    {
        foreach (KeyValuePair<Vector2Int, FieldTileData> tile in Tiles)
        {
            // By default - occupied tiles are ignored
            bool tileIgnored = tile.Value.IsOccupied();

            // If we are iterating filled tiles, take only the occupied ones
            if(iterateFilledTiles)
            {
                tileIgnored = !tileIgnored;
            }

            if (tileIgnored)
            {
                continue;
            }

            yield return tile;
        }

        yield break;
    }

    public Field CopyField()
    {
        Field field = new Field();

        Dictionary<Vector2Int, FieldTileData> copiedTileField = new Dictionary<Vector2Int, FieldTileData>();

        // Copy field tiles
        foreach (KeyValuePair<Vector2Int, FieldTileData> tile in Tiles)
        {
            copiedTileField.Add(tile.Key, new FieldTileData(tile.Value));
        }

        
        field.Tiles = copiedTileField;

        // Copy manipulation stack
        foreach(TileManipulationStackEntry manipulationStackEntry in TileManipulationStack)
        {
            field.TileManipulationStack.Push(manipulationStackEntry);
        }

        return field;
    }

    public override string ToString()
    {
        string builtStr = "Field representation (click for detail):\n";

        int fieldWidth = GetFieldMaxX();

        foreach((Vector2Int, FieldTileData) tile in EnumerateTopToBottom())
        {
            var (tilePosition, tileData) = tile;

            bool isFirstTileInRow = tilePosition.x == 0;
            bool isLastTileInRow = tilePosition.x == fieldWidth;


            string leftBracket = isFirstTileInRow ? "|" : "";
            string playerMarker = tileData.IsOccupied() ? StaticPlayerInfo.INSTANCE.GetPlayerInfoByID(tileData.TilePlayer.Value).StringPlayerRepresentation : StaticPlayerInfo.EMPTY_TILE_INDICATOR;
            string newline = isLastTileInRow ? "\n" : "";

            builtStr += $"{leftBracket}{playerMarker}|{newline}";
        }

        return builtStr;
    }

    public IEnumerable<(Vector2Int, FieldTileData)> EnumerateTopToBottom()
    {
        var maxValues = GetFieldMaxValues();
        int fieldWidth = maxValues.width;
        int fieldHeight = maxValues.height;

        for(int y = fieldHeight; y >= 0; --y)
        {
            for(int x = 0; x <= fieldWidth; ++x)
            {
                Vector2Int pos = new Vector2Int(x, y);
                yield return (pos, GetTile(pos));
            }
        }

        yield break;
    }

    public int GetFieldMaxX()
    {
        return GetFieldMaxValues().width;
    }

    public int GetFieldMaxY()
    {
        return GetFieldMaxValues().height;
    }

    private (int width, int height) GetFieldMaxValues()
    {
        if (GetTilesCount() == 0)
        {
            return (0, 0);
        }

        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (Vector2Int tilePosition in Tiles.Keys)
        {
            maxX = Math.Max(tilePosition.x, maxX);
            maxY = Math.Max(tilePosition.y, maxY);
        }

        return (maxX, maxY);
    }

    public bool DataEquals(object obj)
    {
        Field objField = obj as Field;
        
        if(objField == null)
        {
            return false;
        }

        bool differentTileCount = GetTilesCount() != objField.GetTilesCount();

        if(differentTileCount)
        {
            return false;
        }

        foreach((Vector2Int tilePosition, FieldTileData data) in EnumerateTopToBottom())
        {
            FieldTileData objTile = objField.GetTile(tilePosition);

            if(!data.DataEquals(objTile))
            {
                return false;
            }
        }

        return true;
    }

}
